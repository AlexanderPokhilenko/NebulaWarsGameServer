using System.Net;
using Code.Common;

namespace Server.Http
{
    public static class HttpListenerFactory
    {
        private static readonly ILog Log = LogManager.CreateLogger(typeof(HttpListenerFactory));
        
        public static HttpListener CreateAndRunHttpListener(int port)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            listener.Start();
            
            Log.Info("Ожидание http подключений на порту "+port);
            
            return listener;
        }
    }
}