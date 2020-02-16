using System.Net;
using UnityEngine;

namespace OldServer.Experimental.Http
{
    public static class HttpListenerFactory
    {
        public static HttpListener CreateAndRunHttpListener(int port)
        {
            Debug.Log("Настройка http слушателя на порту "+port);

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            listener.Start();
            
            Debug.Log("Ожидание http подключений на порту "+port);

            return listener;
        }
    }
}