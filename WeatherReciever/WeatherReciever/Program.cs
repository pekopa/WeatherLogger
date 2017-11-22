using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WeatherReciever
{
    class Program
    {

        static void Main(string[] args)
        {



            using (UdpClient socket = new UdpClient(new IPEndPoint(IPAddress.Any, 7000)))
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
                while (true)
                {
                    Console.WriteLine($"Waiting for broadcast {socket.Client.LocalEndPoint}");

                    byte[] datagramReceived = socket.Receive(ref remoteEndPoint);

                    string message = Encoding.ASCII.GetString(datagramReceived, 0, datagramReceived.Length);
                    string[] parts = message.Split(' ');
                    Console.WriteLine(message);

                    HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
                    using (var client = new HttpClient(handler))
                    {
                        client.BaseAddress = new Uri("http://restfullservicefordatalogger.azurewebsites.net");
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            string serializedData =
                                "{\"Humidity\":1.1,\"Pressure\":11.1,\"Temperature\":11.1,\"TimeStamp\":\"" +
                                DateTime.Now + "\",\"WindSpeed\":1.1}";
                            StringContent content =
                                new StringContent(serializedData, Encoding.UTF8, "application/json");

                            var response = client.PostAsync("Weather.svc/WeatherMeasurements/", content).Result;
                            string result = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine(result);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }                       


                        //IService1 asd = new Service1Client("BasicHttpsBinding_IService1");
                        //try
                        //{
                        //    int response = asd.AddSensorInfo(int.Parse(parts[1]), int.Parse(parts[3]), int.Parse(parts[5]), int.Parse(parts[7]));
                        //    Console.WriteLine(response + " - Lines affected");
                        //    Console.WriteLine(message);
                        //    Console.WriteLine();
                        //}
                        //catch (Exception e)
                        //{
                        //    Console.WriteLine(e);
                        //}
                    }
                }
            }
        }
        
    }
}

