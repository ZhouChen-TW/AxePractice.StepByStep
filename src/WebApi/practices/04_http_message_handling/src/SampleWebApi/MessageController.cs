using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace SampleWebApi
{
    public class MessageController : ApiController
    {
        public HttpResponseMessage Get()
        {
            #region Please modify the code to pass the test

            // Please note that you may have to run this program in IIS or IISExpress first in
            // order to pass the test.
            // You can add new files if you want. But you cannot change any existed code.

            IContentNegotiator contentNegotiator = Configuration.Services.GetContentNegotiator();
            var result = contentNegotiator.Negotiate(typeof(object), Request, Configuration.Formatters);
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }

            return Request.CreateResponse(
                HttpStatusCode.OK,
                new MessageDto
                {
                    Message = "Hello"
                },
                result.Formatter,
                result.MediaType);

            #endregion
        }
    }

    public class MessageDto
    {
        public string Message { get; set; }
    }
}