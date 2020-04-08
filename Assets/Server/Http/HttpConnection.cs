using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using ZeroFormatter;

namespace Server.Http
{
    public sealed class HttpConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpConnection));
        
        private HttpListener listener;
        private readonly MatchDataMessageHandler matchDataMessageHandler;

        public HttpConnection(MatchDataMessageHandler matchDataMessageHandler)
        {
            this.matchDataMessageHandler = matchDataMessageHandler;
        }
        
        public async Task StartListenHttp(int port)
        {
            listener = HttpListenerFactory.CreateAndRunHttpListener(port);
            await StartEndlessCycle();
        }

        private async Task StartEndlessCycle()
        {
             while(true)
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
                
                // Log.Info($"Client data content length {request.ContentLength64}");
                
                Stream inputStream = request.InputStream;
                byte[] data;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    inputStream.CopyTo(memoryStream);
                    data = memoryStream.ToArray();
                }
               
                inputStream.Close();

                GameRoomValidationResult result = HandleBytes(data);

                if (result != null)
                {
                    if (result.ResultEnum != GameRoomValidationResultEnum.Ok)
                    {
                        Log.Error(result.ResultEnum);
                    }
                    byte[] responseData = ZeroFormatterSerializer.Serialize(result);
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = responseData.Length;
                    context.Response.OutputStream.Write(responseData,0,responseData.Length);    
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

        private GameRoomValidationResult HandleBytes(byte[] data)
        {
            BattleRoyaleMatchData matchData = ZeroFormatterSerializer.Deserialize<BattleRoyaleMatchData>(data);
            return matchDataMessageHandler.Handle(matchData);
        }
    }
}
