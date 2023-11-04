using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Downloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                List<string> argList = args.ToList();

                String url = argList[0];
                String filename = argList[1];

                string urlPattern = @"^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$";
                string filenamePattern = @"^[A-Za-z0-9_\-]+\.[A-Za-z0-9]+$";

                bool parseURL = Regex.IsMatch(url, urlPattern);
                bool parseFilename = Regex.IsMatch(filename, filenamePattern);

                if (parseURL && parseFilename)
                    Downloader.Init(url, filename);
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (!parseURL)
                        Console.WriteLine("URL does not have valid format");
                    if (!parseFilename)
                        Console.WriteLine("Filename does not have valid format");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Specify args params");
                Console.WriteLine("Example: Downloader <URL> <FILENAME.EXTENSION>");
                Console.ResetColor();
            }
        }
    }
}
