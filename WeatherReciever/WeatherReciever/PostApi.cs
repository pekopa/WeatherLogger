using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeatherReciever
{
    public static class PostApi
    {
        private const string ServerUrl = "http://restfullservicefordatalogger.azurewebsites.net";
        public static string PostData(string url, string objectToPost)
        {
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(ServerUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {                    
                    StringContent content = new StringContent(objectToPost, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(url, content).Result;
                    string result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
    }
}
