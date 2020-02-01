using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using NetworkLibrary.NetworkLibrary.Http;
using ZeroFormatter;

//TODO нужно ли оборачивать stream в using?

namespace AmoebaBattleServer01.Experimental.Http
{
    public class HttpListenerWrapper
    {
        HttpListener listener;
        private HttpMessageHandlers messageHandlers;

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
                 Console.WriteLine("Отправлен ответ по http");
             }
             // ReSharper disable once FunctionNeverReturns
        }

        private async Task HandleNextRequest()
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                
                Console.WriteLine("Client data content length {0}", request.ContentLength64);
                
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
                    byte[] responseData = ZeroFormatterSerializer.Serialize(result);
                    context.Response.ContentLength64 = responseData.Length;
                    context.Response.OutputStream.Write(responseData,0,responseData.Length);    
                }
                else
                {
                    throw new Exception("Отправка пустого ответа гейм матчеру запрещена.");
                }

                context.Response.StatusCode = 200;
                context.Response.OutputStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Брошено исключение при обработке http запроса "+e.Message);
            }
        }

        protected virtual GameRoomValidationResult HandleBytes(byte[] data)
        {
            GameRoomData roomData = ZeroFormatterSerializer.Deserialize<GameRoomData>(data);
            return messageHandlers.Handle(roomData);
        }
    }
}
