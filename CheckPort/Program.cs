using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace CheckPort
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int len = args.Length;

            if(len == 2 || len == 3)
            {
                String port = args[0];
                String type = args[1];
                String time = null;

                if(len == 3)
                {
                    time = args[2];
                }


                string singlePortPattern = @"^(?:[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$";
                string multiplePortsPattern = @"^(?:\d{1,5}(?:,\d{1,5})*|\d{1,5}-\d{1,5})$";
                string typePattern = "^(tcp|udp)$";
                string numberPattern = @"^\d+$";


                bool typePort = Regex.IsMatch(type, typePattern);

                if(len==2)
                {
                    bool parsePort = Regex.IsMatch(port, singlePortPattern);

                    if (parsePort && typePort)
                        CheckPort.Init(int.Parse(port), type);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (!parsePort)
                            Console.WriteLine("Port is not a valid number");
                        if (!typePort)
                            Console.WriteLine("Type is not a valid protocol (tcp|udp)");
                        Console.ResetColor();
                    }
                }
                else if(len==3)
                {
                    bool parsePorts = Regex.IsMatch(port, multiplePortsPattern);
                    bool parseTime = Regex.IsMatch(time, numberPattern);

                    if (parsePorts && typePort && parseTime)
                    {
                        string listPorts = port;
                        int timems = int.Parse(time);

                        if (listPorts.Contains(","))
                        {
                            string[] ports = listPorts.Split(',');

                            foreach (string p in ports)
                            {
                                CheckPort.Init(int.Parse(p), type);
                                Thread.Sleep(timems);
                            }
                        }
                        else if (listPorts.Contains("-"))
                        {
                            string[] ports = listPorts.Split('-');

                            int min = int.Parse(ports[0]);
                            int max = int.Parse(ports[1]);

                            if (min >= max)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Min port is higher than max port");
                                Console.ResetColor();
                            }
                            else
                            {
                                for (int i = min; i <= max; i++)
                                {
                                    CheckPort.Init(i, type);
                                    Thread.Sleep(timems);
                                }

                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (!parsePorts)
                            Console.WriteLine("Port is not a valid sequence");
                        if (!typePort)
                            Console.WriteLine("Type is not a valid protocol (tcp|udp)");
                        if (!parseTime)
                            Console.WriteLine("Type is not a valid number");
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Specify args params");
                Console.WriteLine("Example: CheckPort <PORT> <TYPE:tcp/udp>");
                Console.WriteLine("Example: CheckPort <MINPORT-MAXPORT> <TYPE:tcp/udp> <TIME MS>");
                Console.WriteLine("Example: CheckPort <PORT1,PORT2,PORTN> <TYPE:tcp/udp> <TIME MS>");
                Console.ResetColor();
            }
        }
    }
}
