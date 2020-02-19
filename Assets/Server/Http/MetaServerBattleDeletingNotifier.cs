using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Server.Utils;
using UnityEngine;

namespace Server.Http
{
    /// <summary>
    /// Отправляет гейм матчеру уведомления при окончании боя.
    /// </summary>
    public class MetaServerBattleDeletingNotifier
    {
        public static readonly ConcurrentQueue<int> GameRoomIdsToDelete = new ConcurrentQueue<int>();

        public async Task StartEndlessLoop()
        {
            while (true)
            {
                TrySendDeleteMessages();
                await Task.Delay(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void TrySendDeleteMessages()
        {
            List<Task> tasks = new List<Task>();
            while (!GameRoomIdsToDelete.IsEmpty)
            {
                GameRoomIdsToDelete.TryDequeue(out int gameRoomId);
                tasks.Add(SendMessageAboutGameOver(gameRoomId));
            }

            if (tasks.Count != 0)
            {
                Task t = Task.WhenAll(tasks);
                t.Wait();                
            }
        }

        private static async Task SendMessageAboutGameOver(int gameRoomId)
        {
            string pathname = "/GameServer/DeleteRoom";
            string query = $"?gameRoomId={gameRoomId.ToString()}";
            await SendDelete(pathname, query);
        }
        
        private static async Task SendDelete(string pathname, string query)
        {
            string url = "https://localhost:53847"+pathname+query;
            
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
                    Log.Info("Брошено исключение при отправке http уведомления о удалении комнаты. "+e.Message);
                }
            }
        }
    }
}