using System;
using System.Net.Http;
using System.Threading.Tasks;
using Code.Common;
using Libraries.Logger;

namespace Server.Http
{
    public class HttpWrapper
    {
        private readonly HttpClient client;
        private readonly ILog log = LogManager.CreateLogger(typeof(HttpWrapper));

        public HttpWrapper()
        {
            client = new HttpClient();
        }
        
        public async Task<bool> HttpDelete(string pathname, string query)
        {
            string url = NetworkGlobals.MatchmakerUrl + pathname + query;
            try
            {
                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    log.Info($"Успешная отправка http delete по url = "+url);
                    return true;
                }
                else
                {
                    throw new Exception($"Не удалось отправить сообщение http delete по url = {url}" +
                                        $" status code ={response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                log.Error("Брошено исключение при отправке http уведомления о удалении комнаты. " + e.Message);
            }
            
            return false;
        }
    }
}