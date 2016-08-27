using System.Collections.Generic;
using SimpleWebServer.WebServer;

namespace SimpleWebServer.RouteHandlers
{
    public class HelloWorldRouteHandler : IRouteHandler
    {
        public Response ProcessRequest(string queryString, Dictionary<string, string> headers, string body)
        {
            return new Response
            {
                Code = 200,
                Body = "Hello world!"
            };
        }
    }
}