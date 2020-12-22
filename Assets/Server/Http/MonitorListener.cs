using Code.Common;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Http
{
    public sealed class MonitorListener
    {
        private readonly HttpListener listener;
        private static readonly ILog Log = LogManager.CreateLogger(typeof(MonitorListener));

        public MonitorListener(int port)
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{port}/Status/");
            listener.Start();
        }

        public CancellationTokenSource StartThread()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Thread thread = new Thread(async () => await EndlessCycle(token))
            {
                IsBackground = true
            };
            thread.Start();
            return cts;
        }

        private async Task EndlessCycle(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await HandleNextRequest();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private async Task HandleNextRequest()
        {
            try
            {
                var context = await listener.GetContextAsync();
                //TODO: отправлять что-то полезное
                byte[] responseData = System.Text.Encoding.UTF8.GetBytes("<HTML><BODY>OK</BODY></HTML>");
                context.Response.StatusCode = 200;
                context.Response.ContentLength64 = responseData.Length;
                await context.Response.OutputStream.WriteAsync(responseData, 0, responseData.Length);
                context.Response.OutputStream.Close();
            }
            catch (Exception e)
            {
                Log.Error("Брошено исключение при обработке http запроса " + e.Message);
            }
        }
    }
}