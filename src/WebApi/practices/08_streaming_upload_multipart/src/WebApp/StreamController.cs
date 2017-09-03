using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApp
{
    public class StreamController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> CreateMultipart()
        {
            #region Please implement the method

            /*
             * Please implement the method to retrive all the files data.
             */

            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Request must be multipart.");
            }

            string root = HttpContext.Current.Server.MapPath("~/myfolder");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                var result = provider.FileData.Select(
                    f =>
                    {
                        var fileName = f.Headers.ContentDisposition.FileName;
                        string localFileName = File.ReadAllText(f.LocalFileName);
                        return $"{fileName}:{localFileName}";
                    });

                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            #endregion
        }
    }
}