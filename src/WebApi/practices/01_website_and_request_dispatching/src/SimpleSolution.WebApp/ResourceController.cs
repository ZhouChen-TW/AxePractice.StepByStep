using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class ResourceController : ApiController
    {
        [AcceptVerbs("Get", "Post", "Put", "Delete")]
        public HttpResponseMessage CanHandler()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}