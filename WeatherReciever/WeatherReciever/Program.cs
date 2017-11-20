using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

