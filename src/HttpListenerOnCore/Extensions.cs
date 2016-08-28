using System.IO;
using System.Net;

namespace HttpListenerOnCore
{
    public static class Extensions
    {
        public static void Set(this HttpListenerResponse response, string message, int statusCode)
        {
            response.StatusCode = statusCode;
            using (var streamWriter = new StreamWriter(response.OutputStream))
            {
             
                streamWriter.WriteLine(message);
            }

                

        }
    }
}
