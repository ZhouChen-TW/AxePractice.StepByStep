using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json.Linq;
using SampleWebApi.Services;

namespace SampleWebApi
{
    /*
     * A RestrictedUacAttribute is a filter to eliminate sensitive information to
     * the client. A resource contains management information that is represented
     * by a collection of links. These links will be represented as a array of
     * objects in JSON. And each object must contains an attribute called 
     * "restricted". If it is true, then it should be eliminated if the client
     * is a normal user. If it is false, then the information can be seen by both
     * normal user and administrators.
     * 
     * NOTE. You are free to add non-public members or methods in the class.
     */
    public class RestrictedUacAttribute : ActionFilterAttribute
    {
        #region Please implement the class to pass the test

        readonly string userIdArgumentName;

        /*
         * The attribute takes an argument of the name of the userId parameter in
         * the route. For example, if the request route definition is 
         * 
         * http://www.base.com/user/{userId}/resource/type
         * 
         * Then the userId parameter name in the route is "userId". The attribute
         * will try resolving the parameter and determine the role of the user by
         * passing the parameter to a RoleRepository. And that is why we ask for
         * it.
         */
        public RestrictedUacAttribute(string userIdArgumentName)
        {
            if (userIdArgumentName == null) throw new ArgumentNullException(nameof(userIdArgumentName));
            this.userIdArgumentName = userIdArgumentName;
        }

        /*
         * The action filter for ASP.NET web API is async nativelly. So we simply
         * abandon the sync version of OnActionExecuted, instead, we will implement
         * the async version directly.
         * 
         * Please carefully implement the method to pass all the tests.
         */
        public override async Task OnActionExecutedAsync(
            HttpActionExecutedContext context,
            CancellationToken token)
        {
            if (!context.Response.IsSuccessStatusCode)
            {
                return;
            }

            var actionArguments = context.ActionContext.ActionArguments;
            object userIdArgument;
            actionArguments.TryGetValue(userIdArgumentName, out userIdArgument);

            var userId = userIdArgument as long?;

            if (!userId.HasValue)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var content = context.Response.Content as ObjectContent;
            if (content == null) return;
            var result = await content.ReadAsAsync<JObject>(token);

            var uacContractService = (RestrictedUacContractService) context.Request.GetDependencyScope()
                .GetService(typeof(RestrictedUacContractService));
            uacContractService.RemoveRestrictedInfo(userId.Value, result);

            context.Response.Content = new ObjectContent(result.GetType(), result, content.Formatter);
        }

        #endregion
    }
}