using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckPort
{
    internal class CheckPort
    {
        public static void Init(int port, String type)
        {
            if(type.Equals("tcp"))
            {
                bool usingPort = IsTCPPortInUse(port);
                Console.WriteLine($"is TCP port {port} using : {usingPort}");
            }
            else if(type.Equals("udp"))
            {
                bool usingPort = IsUdpPortInUse(port);
                Console.WriteLine($"is UDP port {port} using : {usingPort}");
            }
        }

        static bool IsTCPPortInUse(int port)
        {
            bool result = false;
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(IPAddress.Loopback, port);
                    result = true;
                }
                catch (SocketException){}
            }
            return result;
        }

        static bool IsUdpPortInUse(int port)
        {
            bool result = false;
            using (UdpClient udpClient = new UdpClient())
            {
                try
                {
                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
                    result = true;
                }
                catch (SocketException){}
            }
            return result;
        }
    }
}
