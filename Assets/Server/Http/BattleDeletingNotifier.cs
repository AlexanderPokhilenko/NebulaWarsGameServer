using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using log4net;
using UnityEngine;

namespace Server.Http
{
    /// <summary>
    /// Отправляет матчеру уведомления при окончании боя.
    /// </summary>
    public static class BattleDeletingNotifier
    {
        public static readonly ConcurrentQueue<int> GameRoomIdsToDelete = new ConcurrentQueue<int>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(BattleDeletingNotifier));

        public static void StartThread()
        {
            Thread thread = new Thread(() => StartEndlessLoop().Wait());
            thread.IsBackground = true;
            thread.Start();
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
            while (!GameRoomIdsToDelete.IsEmpty)
            {
                GameRoomIdsToDelete.TryDequeue(out int gameRoomId);
                await SendMessageAboutGameOver(gameRoomId);
            }
        }

        private static async Task SendMessageAboutGameOver(int gameRoomId)
        {
            string pathname = "/GameServer/DeleteRoom";
            string query = $"?gameRoomId={gameRoomId.ToString()}";
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
                        Log.Info("Успешная я о удалении комнаты");
                    else
                        throw new Exception("Не удалось отправить сообщение о удалении комнаты");
                    
                }
                catch (Exception e)
                {
                    Log.Error("Брошено исключение при отправке http уведомления о удалении комнаты. "+e.Message);
                }
            }
        }
    }
}