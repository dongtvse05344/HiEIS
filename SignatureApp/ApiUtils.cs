using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SignatureApp
{
    public static class ApiUtils
    {
        private const string APP_DOMAIN = "https://localhost:44335/";

        public static async Task<TResponse> PostDataAsync<TResponse>(string apiPath, object data)
        {
            using (var handler = new WebRequestHandler())
            {
                using (var httpClient = new HttpClient(handler))
                {
                    var apiUri = new Uri(APP_DOMAIN);
                    apiUri = new Uri(apiUri, apiPath);

                    var jsonContent = JsonConvert.SerializeObject(data);
                    var reqContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(apiUri, reqContent).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    var resContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<TResponse>(resContent);
                    return result;
                }
            }
        }


        public static async Task<Stream> LoadFileAsync(string filePath)
        {
            using (var handler = new WebRequestHandler())
            {
                using (var httpClient = new HttpClient(handler))
                {
                    var apiUri = new Uri(APP_DOMAIN);
                    apiUri = new Uri(apiUri, filePath);

                    var response = await httpClient.GetStreamAsync(apiUri).ConfigureAwait(false);
                    var ms = new MemoryStream();
                    await response.CopyToAsync(ms);
                    return ms;
                }
            }
        }

        public static async Task<TResponse> UploadFile<TResponse>(string url, string fileName, params KeyValuePair<string, object>[] param)
        {
            var client = new HttpClient();
            var requestContent = new MultipartFormDataContent();
            //    here you can specify boundary if you need---^

            var fileContent = new ByteArrayContent(File.ReadAllBytes(fileName));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");

            requestContent.Add(fileContent, "file", Path.GetFileName(fileName));
            foreach (var item in param)
            {
                var content = new StringContent(item.Value.ToString(), Encoding.UTF8);
                requestContent.Add(content, item.Key);
            }
            var uri = new Uri(APP_DOMAIN);
            uri = new Uri(uri, url);
            var response = await client.PostAsync(uri, requestContent).ConfigureAwait(false)
                ;
            response.EnsureSuccessStatusCode();
            var resContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResponse>(resContent);
            return result;
        }
    }
}
