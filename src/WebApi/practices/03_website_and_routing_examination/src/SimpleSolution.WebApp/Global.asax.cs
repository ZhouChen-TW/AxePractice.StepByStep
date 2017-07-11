using System;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            HttpConfiguration httpConfiguration = GlobalConfiguration.Configuration;
            Bootstrapper.Init(httpConfiguration);
        }
    }
}