using System;
using HttpListenerOnCore.RouteHandlers;
using HttpListenerOnCore.WebserverCore;

namespace HttpListenerOnCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webServer = new WebServerOnHttpListener
            {
                Port = 5001,
                Address = "localhost"
            };


            webServer.SetHandlerForRoute("^/$", HttpVerb.Get, new HelloWorldRouteHandler());        


            webServer.Start();
            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для завершения работы");
            Console.ReadKey();
            webServer.Stop();
        }
    }
}
