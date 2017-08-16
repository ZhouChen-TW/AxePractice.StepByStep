using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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

            var data = new {message = "Hello"};

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = Request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json"))
                    ? new ObjectContent(data.GetType(), data, new JsonMediaTypeFormatter())
                    :new ObjectContent(typeof(string), data.message, new XmlMediaTypeFormatter())
            };

            return responseMessage;

            #endregion
        }
    }
}