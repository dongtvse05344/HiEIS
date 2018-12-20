using HiEIS.Businesses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HiEIS.Controllers
{
    public class ReceiveSignedPDFController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            //// Check whether the POST operation is MultiPart?
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            // Prepare CustomMultipartFormDataStreamProvider in which our multipart form
            // data will be loaded.
            string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Files/Signed");

            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            List<string> files = new List<string>();
            try
            {
                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider.
                await Request.Content.ReadAsMultipartAsync(provider);
                int t = 0;
                var business = new InvoiceBusiness();
                var emailBusiness = new EmailBusiness();
                foreach (MultipartFileData file in provider.FileData)
                {
                    String[] temps = file.LocalFileName.Split('-', '/', '\\');
                    int Id = int.Parse(temps[temps.Length - 2]);
                    files.Add(Id + "-" + temps[temps.Length - 1]);
                    //var invoice = _invoiceService.GetInvoice(_ => _.ERPKey.Equals(Id), _ => _.Customer);
                    var localPath = "/Files/Signed/" + files[t++];
                    var signedInvoice = business.Sign(Id, localPath);

                    String path = HttpContext.Current.Server.MapPath("~" + localPath);
                    //SendMail(fileUrl, invoice.Customer);
                }
                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }
}
