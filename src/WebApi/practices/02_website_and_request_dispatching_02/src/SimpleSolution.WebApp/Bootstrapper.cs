using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace SimpleSolution.WebApp
{
    public class Bootstrapper
    {
        public static void Init(HttpConfiguration configuration)
        {
            // Note. Since response message generation is out of scope
            // of our test. So I have create an extension method called
            // Request.Text(HttpStatusCode, string) to help you generating
            // a textual response.
            configuration.Routes.MapHttpRoute(
                "get by user dependents",
                "users/dependents",
                new {controller = "User", action = "GetByUserDependents" },
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});

            configuration.Routes.MapHttpRoute(
                "get user dependents by id",
                "users/{id}/dependents",
                new {controller = "User", action = "GetUserDependentsById" },
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get), id = @"\d+"});

            configuration.Routes.MapHttpRoute(
                "get by user name",
                "users",
                new {controller = "User", action = "GetByUserName"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});

            configuration.Routes.MapHttpRoute(
                "get by user id",
                "users/{id}",
                new {controller = "User", action = "GetByUserId"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get), id = @"\d+"});

            configuration.Routes.MapHttpRoute(
                "update by user id",
                "users/{id}",
                new {controller = "User", action = "UpdateByUserId"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Put), id = @"\d+"});
        }
    }
}