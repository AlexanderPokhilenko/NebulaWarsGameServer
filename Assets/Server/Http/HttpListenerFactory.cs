using System.Net;
using log4net;

namespace Server.Http
{
    public static class HttpListenerFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpListenerFactory));
        
        public static HttpListener CreateAndRunHttpListener(int port)
        {
            Log.Info("Настройка http слушателя на порту "+port);

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            listener.Start();
            
            Log.Info("Ожидание http подключений на порту "+port);

            return listener;
        }
    }
}