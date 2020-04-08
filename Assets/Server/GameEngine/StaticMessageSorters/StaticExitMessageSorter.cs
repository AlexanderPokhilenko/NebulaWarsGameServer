using System;
using System.Collections.Concurrent;
using System.Linq;
using log4net;

namespace Server.GameEngine.Experimental
{
    public static class StaticExitMessageSorter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StaticExitMessageSorter));
        //PlayerId, secretKey
        private static readonly ConcurrentBag<int> ConcurrentBag = new ConcurrentBag<int>();
        
        public static void AddExitMessage(int playerId)
        {
            Log.Warn(nameof(AddExitMessage));
            if (!ConcurrentBag.Contains(playerId))
            {
                Log.Warn(nameof(AddExitMessage)+" добавление в список");
                ConcurrentBag.Add(playerId);
            }
        }

        public static void Spread()
        {
            while (!ConcurrentBag.IsEmpty)
            {
                ConcurrentBag.TryTake(out int playerId);
                Log.Warn($"{nameof(playerId)} {playerId}");
                if (GameEngineTicker.MatchStorageFacade.TryGetMatchByPlayerId(playerId, out var match))
                {
                    Contexts contexts = match.Contexts;

                    if (contexts != null)
                    {
                        var inputEntity = contexts.input.CreateEntity();
                        inputEntity.AddPlayerExit(playerId);
                        Log.Warn("Успешное добавление сущности");
                    }
                    else
                    {
                        throw new Exception("Пришло сообщение о выходе из матча от игрока, который не" +
                                            " зарегистрирован");
                    }
                }
                else
                {
                    Log.Error($"Не удалось найти матч для игрока {nameof(playerId)} {playerId}");
                }
            }   
        }
    }
}