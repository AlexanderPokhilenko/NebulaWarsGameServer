using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Code.Common;
using Code.Common.Logger;

using NetworkLibrary.NetworkLibrary.Http;
using ZeroFormatter;

namespace Server.Http
{
    public sealed class MatchmakerListener
    {
        private readonly HttpListener listener;
        private readonly MatchModelMessageHandler matchModelMessageHandler;
        private static readonly ILog Log = LogManager.CreateLogger(typeof(MatchmakerListener));

        public MatchmakerListener(MatchModelMessageHandler matchModelMessageHandler, int port)
        {
            listener = HttpListenerFactory.CreateAndRunHttpListener(port);
            this.matchModelMessageHandler = matchModelMessageHandler;
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
             while(!cancellationToken.IsCancellationRequested)
             {
                 await HandleNextRequest();
             }
             // ReSharper disable once FunctionNeverReturns
        }

        private async Task HandleNextRequest()
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                
                Stream inputStream = request.InputStream;
                byte[] data;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await inputStream.CopyToAsync(memoryStream);
                    data = memoryStream.ToArray();
                }
               
                inputStream.Close();

                BattleRoyaleMatchModel matchModel = ZeroFormatterSerializer.Deserialize<BattleRoyaleMatchModel>(data);
                GameRoomValidationResult result = matchModelMessageHandler.Handle(matchModel);
                

                if (result != null)
                {
                    if (result.ResultEnum != GameRoomValidationResultEnum.Ok)
                    {
                        Log.Error(result.ResultEnum);
                    }
                    byte[] responseData = ZeroFormatterSerializer.Serialize(result);
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = responseData.Length;
                    await context.Response.OutputStream.WriteAsync(responseData,0,responseData.Length);    
                    context.Response.OutputStream.Close();
                }
                else
                {
                    throw new Exception("Отправка пустого ответа гейм матчеру запрещена.");
                }
            }
            catch (Exception e)
            {
                Log.Error("Брошено исключение при обработке http запроса "+e.Message);
            }
        }
    }
}
