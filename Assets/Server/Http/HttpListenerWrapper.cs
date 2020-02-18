using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEngine;
using ZeroFormatter;

namespace Server.Http
{
    public sealed class HttpListenerWrapper
    {
        private HttpListener listener;
        private readonly HttpMessageHandlers messageHandlers;

        public HttpListenerWrapper()
        {
            messageHandlers = new HttpMessageHandlers();
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
                 Log.Info("Отправлен ответ по http");
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
                    Log(result);
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

        private void Log(GameRoomValidationResult result)
        {
            Log.InfoWarning(result.ResultEnum.ToString());
            Log.InfoWarning(result.ProblemPlayersIds?.Length);
        }

        private GameRoomValidationResult HandleBytes(byte[] data)
        {
            GameRoomData roomData = ZeroFormatterSerializer.Deserialize<GameRoomData>(data);
            return messageHandlers.Handle(roomData);
        }
    }
}
