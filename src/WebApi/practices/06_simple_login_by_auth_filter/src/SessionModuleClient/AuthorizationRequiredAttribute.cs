using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SessionModuleClient
{
    public class AuthorizationRequiredAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            #region Please implement the method

            /*
             * This authorization attribute will try checking if IPrincipal is valid.
             * If it is not valid, set the response to an unauthorized status. That
             * means, all users that is authenticated is allowd to access resources
             * annotated by this attribute.
             */
            var claimsPrincipal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            return Task.CompletedTask;
            #endregion
        }
    }
}