using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LocalApi
{
    public class HttpServer : HttpMessageHandler
    {
        #region Please implement the following method to pass the test

        /*
         * An http server is an HttpMessageHandler that accept request and create response.
         * You can add non-public fields and members for help but you should not modify
         * the public interfaces.
         */
        readonly HttpConfiguration httpConfiguration;
        public HttpServer(HttpConfiguration configuration)
        {
            httpConfiguration = configuration;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var route = httpConfiguration.Routes.GetRouteData(request);
            if (route == null) return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
            try
            {
                var httpResponseMessage = ControllerActionInvoker.InvokeAction(
                    route,
                    httpConfiguration.CachedControllerTypes,
                    httpConfiguration.DependencyResolver,
                    httpConfiguration.ControllerFactory);

                return Task.FromResult(httpResponseMessage);
            }
            catch
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        #endregion
    }
}