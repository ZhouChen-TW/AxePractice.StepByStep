using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace SessionModuleClient
{
    public class AuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get; } = false;

        public bool RedirectToLoginOnChallenge { get; set; }

        public async Task AuthenticateAsync(
            HttpAuthenticationContext context,
            CancellationToken cancellationToken)
        {
            #region Please implement the following method

            /*
             * We need to create IPrincipal from the authentication token. If
             * we can retrive user session, then the structure of the IPrincipal
             * should be in the following form:
             *
             * ClaimsPrincipal
             *   |- ClaimsIdentity (Primary)
             *        |- Claim: { key: "token", value: "$token value$" }
             *        |- Claim: { key: "userFullName", value: "$user full name$" }
             *
             * If user session cannot be retrived, then the context principal
             * should be an empty ClaimsPrincipal (unauthenticated).
             */
            if(context == null) return;

            var token = GetSessionToken(context);
            if (token == null)
            {
                context.Principal = new ClaimsPrincipal();
                return;
            }

            var session = await GetSession(context, token, cancellationToken);
            if (session == null)
            {
                context.Principal = new ClaimsPrincipal();
                return;
            }
            context.Principal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim("token", token),
                        new Claim("userFullName", session.UserFullname)
                    }, "authentication")
            );

            #endregion
        }

        public Task ChallengeAsync(
            HttpAuthenticationChallengeContext context,
            CancellationToken cancellationToken)
        {
            #region Please implement the following method

            /*
             * The challenge method will try checking the configuration of
             * RedirectToLoginOnChallenge property. If the value is true,
             * then it will replace the response to redirect to login page.
             * And if the value is false, then simply keeps the original
             * response.
             */

            if (RedirectToLoginOnChallenge)
            {
                var actionResult = new RedirectToLoginPageIfUnauthorizedResult(context.Request, context.Result);
                context.Result = actionResult;
            }
            return Task.CompletedTask;
            #endregion
        }

        static async Task<UserSessionDto> GetSession(HttpAuthenticationContext context, string token, CancellationToken cancellationToken)
        {
            var client = (HttpClient)context.Request.GetDependencyScope().GetService(typeof(HttpClient));

            Uri requestRequestUri = context.Request.RequestUri;
            var responseMessage = await client.GetAsync(
                $"{requestRequestUri.Scheme}://{requestRequestUri.UserInfo}{requestRequestUri.Authority}/session/{token}",
                cancellationToken);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var session = await responseMessage.Content.ReadAsAsync<UserSessionDto>(
                context.ActionContext.ControllerContext.Configuration.Formatters,
                cancellationToken);
            return session;
        }

        static string GetSessionToken(HttpAuthenticationContext context)
        {
            var sessionCookie = GetSessionCookie(context);
            string token = sessionCookie?.Value;
            return string.IsNullOrEmpty(token) ? null : token;
        }

        static CookieState GetSessionCookie(HttpAuthenticationContext context)
        {
            const string SessionCookieKey = "X-Session-Token";
            var cookieHeaderValues = context.Request.Headers.GetCookies(SessionCookieKey);
            var sessionCookie = cookieHeaderValues.SelectMany(chv => chv.Cookies)
                .FirstOrDefault(c => c.Name == SessionCookieKey);
            return sessionCookie;
        }
    }
}