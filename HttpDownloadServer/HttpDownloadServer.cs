using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpDownloadServer
{
    internal class HttpDownloadServer
    {
        public static void Init(Int32 port)
        {
            string directoryToServe = AppDomain.CurrentDomain.BaseDirectory; 
            string prefix = $"http://0.0.0.0:{port}/"; 

            try
            { 
                using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add(prefix);
                listener.Start();
                Console.WriteLine($"HTTP server running in {prefix}");

                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;

                    string filePath = directoryToServe + request.RawUrl;

                    if (File.Exists(filePath))
                    {
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        response.ContentLength64 = fileBytes.Length;
                        response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
                        response.Close();
                    }
                    else
                    {
                        string responseString = "File not found";
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                        response.Close();
                    }
                }
            }
            }
            catch (Exception) { }
        }
    }
}
