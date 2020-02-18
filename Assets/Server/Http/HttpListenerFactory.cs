using System.Net;
using Server.Utils;
using UnityEngine;

namespace Server.Http
{
    public static class HttpListenerFactory
    {
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