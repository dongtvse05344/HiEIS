using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using Microsoft.Win32;


namespace SigningCertificate.Utils
{
    public class Utils
    {
        const string fileNew = @"D:\Signed\signed-{0}";
        public static X509Certificate2 GetCertificate()
        {
            X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
                X509Certificate2Collection findResult = certificatesInStore.Find(X509FindType.FindBySubjectName, "VN", true);
                X509Certificate2 clientCertificate = null;
                if (findResult.Count > 0)
                {
                    clientCertificate = findResult[0];
                }
                else
                {
                    throw new Exception("Không nhận diện được chữ kí số, vui lòng kiểm tra lại");
                }
                return clientCertificate;
            }
            catch
            {
                throw;
            }
            finally
            {
                userCaStore.Close();
            }
        }
        public static ByteArrayContent SignWithThisCert(X509Certificate2 cert, string fileInputPath, string type)
        {
            string SourcePdfFileName = fileInputPath;
            var fileName = Path.GetFileName(SourcePdfFileName);
            string DestPdfFileName = String.Format(fileNew, fileName);
            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
            IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");
            PdfReader pdfReader = new PdfReader(SourcePdfFileName);
            //PdfReader pdfReader = new PdfReader(streamFile);
            FileStream signedPdf = new FileStream(DestPdfFileName, FileMode.Create);  //the output pdf file
            PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
            //signatureAppearance.SetVisibleSignature("Signature2");
            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            //signatureAppearance.Layer2Text = "Được ký bởi" + cert.GetName().ToString();
            switch (type)
            {
                case "Invoice":
                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(450, 150, 600, 250), pdfReader.NumberOfPages, null);
                    break;
                case "Sheet":
                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 0, 0, 0), pdfReader.NumberOfPages, null);
                    break;
            }
            //signatureAppearance.Layer2Font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN);
            BaseFont unicode =
                        BaseFont.CreateFont("c:/windows/fonts/times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            signatureAppearance.Layer2Font = new iTextSharp.text.Font(unicode);
            MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

            return new ByteArrayContent(ReadFully(fileName));
        }

        public static byte[] ReadFully(String fileName)
        {
            using (FileStream fileStream = File.Open(String.Format(fileNew, fileName), FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Đăng ký Registry để sử dụng URL protocol
        /// </summary>
        /// <param name="appPath">Đường dẫn app</param>
        /// <param name="protocolName">Tên protocol - thường là tên app</param>
        static void RegisterProtocol(string appPath, string protocolName)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(protocolName); // app protocol subkey

            if (key == null) // register if not
            {
                key = Registry.ClassesRoot.CreateSubKey(protocolName); // app protocol subkey
                key.SetValue(string.Empty, "URL: " + protocolName + " Protocol");
                key.SetValue("URL Protocol", string.Empty);

                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, appPath + " " + "%1");
                // %1 - param
            }

            key.Close();
        }

        /// <summary>
        /// Lấy token từ server và lưu xuống file text
        /// </summary>
        /// <param name="baseAddress">địa chỉ server ex: (http://localhost:55356)</param>
        /// <param name="username">tên đăng nhập</param>
        /// <param name="password">mật khẩu</param>
        /// <returns>chuỗi token</returns>
        static string GetToken(string baseAddress, string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                // api domain
                client.BaseAddress = new Uri(baseAddress);

                // request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // request body
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });

                // send request
                var response = client.PostAsync("Token", content).Result;

                // response content
                var json = response.Content.ReadAsStringAsync().Result;

                // get token from response
                if (response.IsSuccessStatusCode)
                {
                    json = json.Replace("{", "");
                    json = json.Replace("}", "");
                    json = json.Replace("\"", "");
                    var rs = json.Split(':', ',');

                    var token = rs[1];
                    var expiredTime = rs[5];

                    // Cần folder token đã được tạo sẵn
                    string resultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token/token.txt");

                    // lưu token xuống file text
                    if (!File.Exists(resultPath))
                    {
                        File.Create(resultPath);
                        TextWriter tw = new StreamWriter(resultPath);
                        tw.WriteLine(token);
                        tw.Write(expiredTime);
                        tw.Close();
                    }
                    else
                    {
                        TextWriter tw = new StreamWriter(resultPath);
                        tw.WriteLine(token);
                        tw.Write(expiredTime);
                        tw.Close();
                    }

                    return token;
                }

                return null;
            }
        }

        /// <summary>
        /// Đọc chuỗi token được lưu từ file text
        /// </summary>
        /// <returns></returns>
        static string ReadToken()
        {
            try
            {
                string resultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token/token.txt");
                TextReader tr = new StreamReader(resultPath);
                // chuỗi token
                var token = tr.ReadLine();
                // thời gian hết hạn
                var expiredTime = double.Parse(tr.ReadLine());
                // last modified
                DateTime modifiedDate = File.GetLastWriteTime(resultPath);
                // Ngày hết hạn
                DateTime expiredDate = modifiedDate.Add(TimeSpan.FromSeconds(expiredTime));

                // kiểm tra thời gian hết hạn
                if (expiredDate < DateTime.Now)
                {
                    return null;
                }

                return token;
            }
            catch (Exception)
            {
                return null;
            }

        }

        // Đọc và ghi file PDF từ server
        static void PDFRead(string baseAddress)
        {
            string token = ReadToken();

            if (string.IsNullOrEmpty(token))
            {
                token = GetToken(baseAddress, "LD", "Aa123456");
            }

            PdfReader reader = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = client.GetAsync("/api/File/GetInvoicePDF?ERPKey=A010000000112848").Result;
                if (response.IsSuccessStatusCode)
                {
                    reader = new PdfReader(response.Content.ReadAsStreamAsync().Result);
                }
            }

            string resultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/pdffile.pdf");
            PdfStamper stamper = new PdfStamper(reader, new FileStream(resultPath, FileMode.Create));
            stamper.Close();
            reader.Close();
        }
    }

}
