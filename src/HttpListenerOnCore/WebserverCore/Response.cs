namespace SimpleWebServer.WebServer
{
    /// <summary>
    ///     Ответ веб-сервера
    /// </summary>
    public class Response
    {
        /// <summary>
        ///     Код ответа
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     Тело ответа
        /// </summary>
        public string Body { get; set; }
    }
}