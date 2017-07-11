using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class Bootstrapper
    {
        public static void Init(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute(
                "resource",
                "resource",
                new {controller = "Resource"});

            configuration.Routes.MapHttpRoute(
                "users",
                "users",
                new { controller = "User" });
        }
    }
}