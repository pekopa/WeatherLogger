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
                    Console.WriteLine("Data From Raspberry PI");
                    Console.WriteLine(message);

                    string[] parts = message.Split(' ');
                    string serializedData = "{\"Humidity\":" + parts[1] + ",\"Pressure\":" + parts[3] + ",\"Temperature\":" + parts[5] + ",\"TimeStamp\":\"" + DateTime.Now + "\",\"WindSpeed\":" + parts[7] + "}";                   
                    string response = PostApi.PostData("Weather.svc/WeatherMeasurements/", serializedData);

                    Console.WriteLine("Response from database");
                    Console.WriteLine(response);                
                }
            }
        }        
    }
}

