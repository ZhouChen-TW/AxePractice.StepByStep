using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class UserController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetByUserDependents()
        {
            return new HttpRequestMessage().Text(HttpStatusCode.OK, "This will get all users's dependents");
        }

        [HttpGet]
        public HttpResponseMessage GetUserDependentsById(long id)
        {
            return new HttpRequestMessage().Text(HttpStatusCode.OK, $"This will get user(id={id})'s dependents");
        }

        [HttpGet]
        public HttpResponseMessage GetByUserName([FromUri(Name = "name")]string name)
        {
            return new HttpRequestMessage().Text(HttpStatusCode.OK, $"user(name={name}) found");
        }

        [HttpGet]
        public HttpResponseMessage GetByUserId(long id)
        {
            return new HttpRequestMessage().Text(HttpStatusCode.OK, $"user(id={id}) found");
        }

        [HttpPut]
        public HttpResponseMessage UpdateByUserId(long id, [FromUri(Name = "name")]string name)
        {
            return new HttpRequestMessage().Text(HttpStatusCode.OK, $"user(id={id})'s name updated to {name}");
        }
    }
}