using System;
using System.Net;

namespace Downloader
{
    internal class Downloader
    {
        public static void Init(string url, string filename)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(url, filename);
                    Console.WriteLine("File downloaded");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error downloading this file : " + ex.Message);
                }
            }
        }
        
    }
}
