using System;
using System.Net;

namespace AmoebaBattleServer01.Experimental.Http
{
    public static class HttpListenerFactory
    {
        public static HttpListener CreateAndRunHttpListener(int port)
        {
            Console.WriteLine("Настройка http слушателя на порту "+port);

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            listener.Start();
            
            Console.WriteLine("Ожидание http подключений на порту "+port);

            return listener;
        }
    }
}