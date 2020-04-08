using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using ZeroFormatter;

namespace Server.Http
{
    public class HttpMediator
    {
        private HttpMessageProcessor httpMessageProcessor = new HttpMessageProcessor();
        
        private GameRoomValidationResult HandleBytes(byte[] data)
        {
            BattleRoyaleMatchData matchData = ZeroFormatterSerializer.Deserialize<BattleRoyaleMatchData>(data);
            return httpMessageProcessor.Handle(matchData);
        }
    }

    

    public sealed class HttpConnection
    {
        private HttpListener listener;
        private readonly MatchDataMessageHandler messageHandler;
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpConnection));

        public HttpConnection()
        {
            messageHandler = new MatchDataMessageHandler();
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

        private void LogGameRoomValidationResult(GameRoomValidationResult result)
        {
            Log.Warn(result.ResultEnum.ToString());
            Log.Warn(result.ProblemPlayersIds?.Length);
        }

        private GameRoomValidationResult HandleBytes(byte[] data)
        {
            BattleRoyaleMatchData matchData = ZeroFormatterSerializer.Deserialize<BattleRoyaleMatchData>(data);
            return messageHandler.Handle(matchData);
        }
    }
}
