using System.Collections.Generic;

namespace HttpListenerOnCore.WebserverCore
{
    /// <summary>
    ///     Обработчки для маршрута
    /// </summary>
    public interface IRouteHandler
    {
        /// <summary>
        ///     Обрабатывает запрос
        /// </summary>
        /// <param name="queryString">адрес запроса</param>
        /// <param name="headers">заголовки запроса</param>
        /// <param name="body">тело запроса</param>
        /// <returns>строка текст ответа</returns>
        Response ProcessRequest(string queryString, Dictionary<string, string> headers, string body);
    }
}