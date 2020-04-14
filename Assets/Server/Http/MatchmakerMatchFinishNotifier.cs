using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using log4net;

namespace Server.Http
{
    /// <summary>
    /// Отправляет матчмейкеру уведомление об окончании боя.
    /// </summary>
    public static class MatchmakerMatchFinishNotifier
    {
        private static readonly ConcurrentQueue<int> MatchIdsToDelete = new ConcurrentQueue<int>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchmakerMatchFinishNotifier));

        public static void SendMessage(int matchId)
        {
            MatchIdsToDelete.Enqueue(matchId);
        }
        
        public static Thread StartThread()
        {
            Thread thread = new Thread(() => StartEndlessLoop().Wait())
            {
                IsBackground = true
            };
            thread.Start();
            return thread;
        }
        
        private static async Task StartEndlessLoop()
        {
            while (true)
            {
                await TrySendDeleteMessages();
                await Task.Delay(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static async Task TrySendDeleteMessages()
        {
            while (!MatchIdsToDelete.IsEmpty)
            {
                MatchIdsToDelete.TryDequeue(out int gameRoomId);
                await SendMessageAboutGameOver(gameRoomId);
            }
        }

        private static async Task SendMessageAboutGameOver(int gameRoomId)
        {
            string pathname = "/GameServer/DeleteRoom";
            string query = $"?MatchId={gameRoomId.ToString()}";
            await SendDelete(pathname, query);
        }
        
        //TODO вынести отсюда далеко
        private static async Task SendDelete(string pathname, string query)
        {
            string url = NetworkGlobals.MatchmakerUrl+pathname+query;
            
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.DeleteAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Info("Удалении комнаты: успешно.");
                    }
                    else
                    {
                        throw new Exception("Не удалось отправить сообщение о удалении комнаты");
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Брошено исключение при отправке http уведомления о удалении комнаты. "+e.Message);
                }
            }
        }
    }
}