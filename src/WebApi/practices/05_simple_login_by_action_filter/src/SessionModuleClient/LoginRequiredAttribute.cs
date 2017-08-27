using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SessionModuleClient
{
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override bool AllowMultiple { get; } = false;

        public override async Task OnActionExecutingAsync(
            HttpActionContext context,
            CancellationToken cancellationToken)
        {
            #region Please implement the method

            // This filter will try resolve session cookies. If the cookie can be
            // parsed correctly, then it will try calling session API to get the
            // specified session. To ease user session access, it will store the
            // session object in request message properties.
            if(context == null) throw new ArgumentNullException(nameof(context));
            var token = GetSessionToken(context);
            if (token == null)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }
            var session = await GetSessionAsync(context, token, cancellationToken);
            if (session == null)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }
            context.Request.SetUserSession(session);

            #endregion
        }

        async Task<UserSessionDto> GetSessionAsync(HttpActionContext context, string token, CancellationToken cancellationToken)
        {
            var client = (HttpClient)context.Request.GetDependencyScope().GetService(typeof(HttpClient));

            Uri requestRequestUri = context.Request.RequestUri;
            var responseMessage = await client.GetAsync(
                $"{requestRequestUri.Scheme}://{requestRequestUri.UserInfo}{requestRequestUri.Authority}/session/{token}",
                cancellationToken);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            var session = await responseMessage.Content.ReadAsAsync<UserSessionDto>(
                context.ControllerContext.Configuration.Formatters,
                cancellationToken);
            return session;
        }

        string GetSessionToken(HttpActionContext context)
        {
            const string SessionCookieKey = "X-Session-Token";
            var cookieHeaderValues = context.Request.Headers.GetCookies(SessionCookieKey);
            CookieState sessionCookie = cookieHeaderValues.SelectMany(chv => chv.Cookies)
                .FirstOrDefault(c => c.Name == SessionCookieKey);
            string token = sessionCookie?.Value;
            return string.IsNullOrEmpty(token) ? null : token;
        }
    }
}