using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleWebServer.WebServer.HttpListener;
using SimpleWebServer.WebServer;
using SimpleWebServer.RouteHandlers;

namespace HttpListenerOnCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webServer = new WebServerOnHttpListener();
            
            webServer.Port = 5001;
            webServer.Address = "localhost";

            webServer.SetHandlerForRoute("^/$", HttpVerb.Get, new HelloWorldRouteHandler());        


            webServer.Start();
            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для завершения работы");
            Console.ReadKey();
            webServer.Stop();
        }
    }
}
