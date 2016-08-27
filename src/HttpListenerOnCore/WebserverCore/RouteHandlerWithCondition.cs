using System.Text.RegularExpressions;

namespace SimpleWebServer.WebServer.HttpListener
{
    public class RouteHandlerWithCondition
    {
        /// <summary>
        ///     Обработчик
        /// </summary>
        public IRouteHandler RouteHandler { get; set; }

        /// <summary>
        ///     Глагол
        /// </summary>
        public HttpVerb Verb { get; set; }

        /// <summary>
        ///     Регулярное выражение для проверки части адреса идущей после имени сайта и порта
        /// </summary>
        public Regex PostfixRegex { get; set; }


        public bool IsMatchByUrl(string url)
        {
            return PostfixRegex.IsMatch(url);
        }

        public bool IsMatchByVerb(string verb)
        {
            switch (verb.ToUpper())
            {
                case "GET":
                    return Verb == HttpVerb.Get;
                case "POST":
                    return Verb == HttpVerb.Post;
                case "PUT":
                    return Verb == HttpVerb.Put;
                case "DELETE":
                    return Verb == HttpVerb.Delete;
            }
            return false;
        }
    }
}