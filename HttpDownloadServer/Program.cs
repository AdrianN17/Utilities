using System.Text.RegularExpressions;
using System;

namespace HttpDownloadServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                String port = args[0];

                string portPattern = @"^(?:[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$";

                bool parsePort = Regex.IsMatch(port, portPattern);

                if (parsePort)
                    HttpDownloadServer.Init(Int32.Parse(port));
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (!parsePort)
                        Console.WriteLine("Port is not a valid Number");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Specify args params");
                Console.WriteLine("Example: HttpDownloadServer <PORT>");
                Console.ResetColor();
            }
        }
    }
}
