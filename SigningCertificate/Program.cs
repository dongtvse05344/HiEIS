using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SigningCertificate
{

    class Program
    {
        static String _uri = "https://localhost:44335/";
        static void Main(string[] args)
        {
            try
            {
                List<String> paths = new List<string>();
                List<String> Ids = new List<string>();
                String type = "Invoice";
                try
                {
                    //type = args[0].ToString();
                    int index = 0;
                    while (true)
                    {
                        paths.Add(_uri + args[index].ToString());
                        index++;
                        Ids.Add(args[index].ToString());
                        index++;
                    }
                }
                catch (Exception e)
                {

                }
                //paths.Add(_uri + "/Files/Invoices/20181220_cN8sxSsl1QAPM.pdf");
                //Ids.Add("2103");
                var cert = Utils.Utils.GetCertificate();
                if (cert == null)
                {
                    throw new Exception("Không nhận diện được chữ kí số, vui lòng kiểm tra lại");
                }
                else
                {
                    using (var client = new HttpClient())
                    using (var content = new MultipartFormDataContent())
                    {
                        for (var i = 0; i < paths.Count(); i++)
                        {
                            var path = paths[i];
                            var pdfByte = Utils.Utils.SignWithThisCert(cert, path, type);
                            var fileName = Path.GetFileName(path);

                            // Make sure to change API address
                            client.BaseAddress = new Uri(_uri);

                            // Add first file content 
                            var fileContent1 = pdfByte;
                            fileContent1.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = Ids[i] + "-" + fileName
                            };
                            content.Add(fileContent1);

                        }
                        Boolean result = false;
                        result = client.PostAsync("/api/ReceiveSignedPDF/Post", content).Result.IsSuccessStatusCode;


                        if (result)
                        {
                            Console.WriteLine("Đã kí thành công !!!");
                        }
                        else
                        {
                            Console.WriteLine("Có lỗi xảy ra, vui lòng thử lại !!!");
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}
