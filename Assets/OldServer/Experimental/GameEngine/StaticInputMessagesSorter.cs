using System;
using System.Collections.Concurrent;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;

namespace AmoebaBattleServer01.Experimental.GameEngine
{
    public static class StaticInputMessagesSorter
    {
        public static readonly ConcurrentDictionary<string, Vector2> MessagesToBeSynchronized =
            new ConcurrentDictionary<string, Vector2>();

        public static void Spread()
        {
            // if(MessagesToBeSynchronized.Count!=0)
                // Console.WriteLine($"В словаре {MessagesToBeSynchronized.Count.ToString()} элементов");


            foreach (var (playerId, joystickPosition) in MessagesToBeSynchronized)
            {
                if (GameEngineMediator.GameSessionsStorage.PlayersToSessions.TryGetValue(playerId, out GameSession gameSession))
                {
                    Contexts contexts = gameSession.Contexts;

                    if (contexts != null)
                    {
                        var inputEntity = contexts.input.CreateEntity();
                        inputEntity.AddPlayerJoystickInput(playerId, joystickPosition.X, joystickPosition.Y);    
                    }
                    else
                    {
                        throw new Exception("Пришло сообщение с вводом от игрока, который не зарегистрирован");
                    }   
                }
            }
            
            MessagesToBeSynchronized.Clear();
        }
    }
}