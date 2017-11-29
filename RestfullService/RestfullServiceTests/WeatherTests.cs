using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RestfullServiceTests
{
    [TestClass()]
    public class WeatherTests
    {
        [TestMethod()]
        public void GetWeatherMeasurementsTest()
        {
            const string serverUrl = "http://restfullservicefordatalogger.azurewebsites.net/";
            string url = "Weather.svc/WeatherMeasurements/";

            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Assert.Fail("Bad statusCode");
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result == "[]")
                    {
                        Assert.Fail("No objects found");
                    }
                }
            }
        }

        [TestMethod()]
        public void GetWeatherMeasurementsByIdTest()
        {
            const string serverUrl = "http://restfullservicefordatalogger.azurewebsites.net/";
            string id = "2";
            string url = $"Weather.svc/WeatherMeasurements/{id}";

            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                
                if (!response.IsSuccessStatusCode)
                {
                   Assert.Fail("Bad statusCode");
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result == "[]")
                    {
                        Assert.Fail("No objects found");
                    }
                }
            }
        }

        [TestMethod()]
        public void GetWeatherMeasurementsBydate()
        {
            const string serverUrl = "http://restfullservicefordatalogger.azurewebsites.net/";
            string dateFrom = "1511784647";
            string DateTo = "1511784647";
            string url = $"Weather.svc/WeatherMeasurements/{dateFrom}/{DateTo}";

            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Assert.Fail("Bad statusCode");
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result == "[]")
                    {
                        Assert.Fail("No objects found");
                    }
                }
            }
        }

        [TestMethod()]
        public void PostAndDeleteWeatherMeasurements()
        {
            const string serverUrl = "http://restfullservicefordatalogger.azurewebsites.net/";         
            string url = "Weather.svc/WeatherMeasurements/";

            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Posting new object
                string serializedData = "{ \"Humidity\":34,\"Pressure\":7,\"Temperature\":12,\"TimeStamp\":1511784647,\"WindSpeed\":39}";
                StringContent content = new StringContent(serializedData, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Assert.Fail("Bad statusCode");
                }
                // extracting id from result
                string objresult = response.Content.ReadAsStringAsync().Result.Replace("\"",String.Empty);
                string[] newObj  = objresult.Split(' ');               
                string newObjId = newObj[1];
                // remove posted object
                string deleteUrl = url + newObjId;
                response = client.DeleteAsync(deleteUrl).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Assert.Fail("Bad statusCode");
                }
                // extracting row affted number
                string rowsaffected = response.Content.ReadAsStringAsync().Result.Replace("\"", String.Empty);
                string[] splitResult = rowsaffected.Split(' ');
                rowsaffected = splitResult[1];
                int rowNumber = int.Parse(rowsaffected);               
                if (rowNumber == 0)
                {
                    Assert.Fail("Object not found");
                }
            }
        }
    }
}