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
    /// Отправляет матчеру уведомления при смерти игрока.
    /// </summary>
    public static class PlayerDeathNotifier
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerDeathNotifier));
        public static readonly ConcurrentQueue<PlayerDeathData> KilledPlayers =
            new ConcurrentQueue<PlayerDeathData>();
        
        public static Thread StartThread()
        {
            Thread thread = new Thread(() => Start().Wait()) {IsBackground = true};
            thread.Start();
            return thread;
        }

        private static async Task Start()
        {
            while (true)
            {
                await TrySendMessages();
                await Task.Delay(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }
        
        private static async Task TrySendMessages()
        {
            while (!KilledPlayers.IsEmpty)
            {
                Log.Info("Отправка уведомления о смерти игрока на матчмейкер");
                KilledPlayers.TryDequeue(out var playerDeathData);
                CheckDeathData(playerDeathData);
                await SendMessageAboutPlayerDeath(playerDeathData);
            }
        }

        private static void CheckDeathData(PlayerDeathData playerDeathData)
        {
            if (playerDeathData.MatchId == 0)
            {
                throw new Exception(nameof(playerDeathData.MatchId));
            }
            
            if (playerDeathData.PlayerId == 0)
            {
                throw new Exception(nameof(playerDeathData.PlayerId));
            }
            
            if (playerDeathData.PlaceInBattle == 0)
            {
                throw new Exception(nameof(playerDeathData.PlaceInBattle));
            }
        }
        
        private static async Task SendMessageAboutPlayerDeath(PlayerDeathData playerDeathData)
        {
            string pathname = "/GameServer/PlayerDeath";
            string query = $"?accountId={playerDeathData.PlayerId}" +
                           $"&placeInBattle={playerDeathData.PlaceInBattle}" +
                           $"&MatchId={playerDeathData.MatchId}";
            await SendDelete(pathname, query);
        }
        
        //TODO вынести отсюда далеко
        private static async Task SendDelete(string pathname, string query)
        {
            string url = Globals.MatchmakerUrl+pathname+query;
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.DeleteAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Info("Успешно отправлено сообщение о смерти игрока");
                    }
                    else
                    {
                        string message = $"Не удалось отправить сообщение о смерти игрока. " +
                                         $"{nameof(response.StatusCode)} {response.StatusCode}";
                        throw new Exception(message);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Брошено исключение при отправке http уведомления о смерти игрока. "+e.Message);
                }
            }
        }
    }
}