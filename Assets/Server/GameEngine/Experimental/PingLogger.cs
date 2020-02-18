using System;
using System.Collections.Concurrent;

namespace Server.GameEngine.StaticMessageSorters
{
    public static class PingLogger
    {
        //в эту коллекцию кладутся/обновляются элемента при получении новых пинг сообщений
        public static readonly ConcurrentDictionary<int, DateTime> LastPingTime =
            new ConcurrentDictionary<int, DateTime>();
        
        public static void Log()
        {
            // DateTime now = DateTime.UtcNow;
            // int maxDelaySeconds = 5;
            // foreach (var pair in LastPingTime)
            // {
            //     if ((now - pair.Value).TotalSeconds > maxDelaySeconds)
            //     {
            //         Log.Info($"Игрок с id = {pair.Key} не присылват пинг уже больше {maxDelaySeconds} секунд");
            //     }
            // }
        }
    }
}