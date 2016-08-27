using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HttpListenerOnCore;

namespace SimpleWebServer.WebServer.HttpListener
{
    public class WebServerOnHttpListener 
    {        
        private const int MaxConnections = 5;
        private readonly List<Task> _connections = new List<Task>();

        private List<RouteHandlerWithCondition> _handlers =
            new List<RouteHandlerWithCondition>();

        private bool _isStarted;
        private System.Net.HttpListener _listener;
        public int Port { get; set; }
        public string Address { get; set; }
        
       

        public void SetHandlerForRoute(string routePattern, HttpVerb verb, IRouteHandler routeHandler)
        {
            if (routeHandler == null)
                throw new ArgumentException("routeHandler");
            var regex = new Regex(routePattern ?? "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var routeHandlerWithCondition = new RouteHandlerWithCondition
            {
                PostfixRegex = regex,
                RouteHandler = routeHandler,
                Verb = verb
            };
            _handlers.Add(routeHandlerWithCondition);
        }

        public void Start()
        {
            if (_isStarted)
                return;

            if (Port == 0)
                throw new Exception("Порт не инициализирован");
            if (String.IsNullOrEmpty(Address))
                throw new Exception("Адрес не инициализирован");

            var prefix = String.Format("http://{0}:{1}/", Address, Port);

            _listener = new System.Net.HttpListener();
            _listener.Prefixes.Add(prefix);
            _listener.Start();

            for (int i = 0; i < MaxConnections; i++)
            {
                AddConnectionForListener();
            }

            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted)
                return;
            _listener.Stop();
            _connections.Clear();
            _handlers.Clear();
            _isStarted = false;
        }

        private void AddConnectionForListener()
        {
            var taskForListening = _listener.GetContextAsync();
            taskForListening.ContinueWith(x =>
            {
                ProcessRequest(x);
                _connections.Remove(x);
                AddConnectionForListener();
            });
            _connections.Add(taskForListening);
        }

        private void ProcessRequest(Task<HttpListenerContext> taskForContext)
        {
            var context = taskForContext.Result;
            var rawUrl = context.Request.RawUrl;
            var requestsWithUrl = _handlers.Where(x => x.IsMatchByUrl(rawUrl)).ToList();
            if (!requestsWithUrl.Any())
            {                
                context.Response.Set("Not found", 404);
                return;
            }

            var requestsWithMethod = requestsWithUrl.Where(x => x.IsMatchByVerb(context.Request.HttpMethod)).ToList();
            if (!requestsWithMethod.Any())
            {
                context.Response.Set("Method not allowed", 405);
                return;
            }

            if (requestsWithMethod.Count > 1)
            {
                context.Response.Set("Multiple handlers match request", 500);
                return;
            }

            var queryString = rawUrl;
            int indexOfQuestion = rawUrl.IndexOf('?');
            if (indexOfQuestion > -1)
            {
                queryString = rawUrl.Substring(indexOfQuestion + 1);
            }

            var headers = context.Request.Headers.AllKeys.ToDictionary(x => x, x => context.Request.Headers[x]);
            string requestBody;
            using (var requestBodyReader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                requestBody = requestBodyReader.ReadToEnd();
            }


            try
            {
                var response = requestsWithMethod.First()
                .RouteHandler.ProcessRequest(queryString, headers,
                    requestBody);
                context.Response.Set(response.Body, response.Code);
            }
            catch (Exception ex)
            {
                context.Response.Set("Internal Server Error", 500);
            }
            
            
        }
    }
}