using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SessionModuleClient
{
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override bool AllowMultiple { get; } = false;
        const string SessionCookieKey = "X-Session-Token";

        public override async Task OnActionExecutingAsync(
            HttpActionContext context,
            CancellationToken cancellationToken)
        {
            #region Please implement the method

            // This filter will try resolve session cookies. If the cookie can be
            // parsed correctly, then it will try calling session API to get the
            // specified session. To ease user session access, it will store the
            // session object in request message properties.
            var cookieHeaderValue = context.Request.Headers.GetCookies(SessionCookieKey).SingleOrDefault();
            if (cookieHeaderValue == null)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }

            var client = (HttpClient)context.Request.GetDependencyScope().GetService(typeof(HttpClient));
            var responseMessage = await client.GetAsync(
                $"{context.Request.RequestUri}session/{cookieHeaderValue[SessionCookieKey].Value}",
                cancellationToken);
            if (!responseMessage.IsSuccessStatusCode)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }
            var userSessionDto = await responseMessage.Content.ReadAsAsync<UserSessionDto>(cancellationToken);
            context.Request.SetUserSession(userSessionDto);

            #endregion
        }
    }
}