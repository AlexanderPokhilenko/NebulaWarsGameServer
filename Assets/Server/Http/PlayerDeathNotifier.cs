using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using log4net;

namespace Server.Http
{
    public struct PlayerDeathData
    {
        public int PlayerId;
        //TODO пирдумать нормальное название для переменной
        public int PlaceInBattle;
    }
    
    /// <summary>
    /// Отправляет матчеру уведомления при смерти игрока.
    /// </summary>
    public static class PlayerDeathNotifier
    {
        public static readonly ConcurrentQueue<PlayerDeathData> KilledPlayerIds = new ConcurrentQueue<PlayerDeathData>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerDeathNotifier));
        
        public static void StartThread()
        {
            Thread thread = new Thread(() => Start().Wait());
            thread.IsBackground = true;
            thread.Start();
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
            while (!KilledPlayerIds.IsEmpty)
            {
                KilledPlayerIds.TryDequeue(out var playerDeathData);
                await SendMessageAboutPlayerDeath(playerDeathData);
            }
        }

        private static async Task SendMessageAboutPlayerDeath(PlayerDeathData playerDeathData)
        {
            string pathname = "/GameServer/PlayerDeath";
            string query = $"?playerId={playerDeathData.PlayerId}&placeInBattle={playerDeathData.PlaceInBattle}";
            await SendDelete(pathname, query);
        }
        
        //TODO вынести отсюда далеко
        private static async Task SendDelete(string pathname, string query)
        {
            string url = Globals.GameMatcheUrl+pathname+query;
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
                        throw new Exception("Не удалось отправить сообщение о смерти игрока");
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