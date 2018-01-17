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
            //using (UdpClient socket = new UdpClient(new IPEndPoint(IPAddress.Any, 7000)))
            //{
            //    IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
            //    while (true)
            //    {
            //        Console.WriteLine($"Waiting for broadcast {socket.Client.LocalEndPoint}");

            //        byte[] datagramReceived = socket.Receive(ref remoteEndPoint);

            //        string message = Encoding.ASCII.GetString(datagramReceived, 0, datagramReceived.Length);
            //        Console.WriteLine("Data From Raspberry PI");
            //        Console.WriteLine(message);

            //        string[] parts = message.Split(' ');
            //        string serializedData = "{\"Humidity\":" + parts[1] + ",\"Pressure\":" + parts[3] + ",\"Temperature\":" + parts[5] + ",\"TimeStamp\":\"" + DateTimeOffset.Now.ToUnixTimeSeconds() + "\",\"WindSpeed\":" + parts[7] + "}";                   
            //        string response = PostApi.PostData("Weather.svc/WeatherMeasurements/", serializedData);

            //        Console.WriteLine("Response from database");
            //        Console.WriteLine(response);                
            //    }
            //}

            Random rnd = new Random();

            for (int i = 1; i < 25; i++)
            {
                string serializedData = "{\"Humidity\":" + (80 + rnd.Next(1, 5)).ToString() + ",\"Pressure\":" + (1000 + rnd.Next(1, 20)).ToString() + ",\"Temperature\":" + (1 + rnd.Next(1, 3)).ToString() + ",\"TimeStamp\":\"" + (1516583714 + i*60*60).ToString() + "\",\"WindSpeed\":" + (4 + rnd.Next(1, 8)).ToString() + "}";
                string response = PostApi.PostData("Weather.svc/WeatherMeasurements/", serializedData);
            }
            
        }
    }
}

