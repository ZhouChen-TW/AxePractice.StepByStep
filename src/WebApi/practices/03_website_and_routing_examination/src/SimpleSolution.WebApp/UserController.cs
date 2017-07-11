using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class UserController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetUserById(long id)
        {
            return new HttpRequestMessage().Text(HttpStatusCode.OK, $"get user by id({id})");
        }
    }
}