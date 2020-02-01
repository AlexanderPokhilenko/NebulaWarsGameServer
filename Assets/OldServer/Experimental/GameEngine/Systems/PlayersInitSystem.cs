using System;
using System.Collections.Concurrent;
using System.Threading;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;

namespace AmoebaBattleServer01.Experimental.GameEngine.Systems
{
    /// <summary>
    /// Спавнит игроков на карте.
    /// </summary>
    public class PlayersInitSystem:IInitializeSystem
    {
        private readonly GameContext gameContext;
        private readonly GameRoomData roomData;

        public PlayersInitSystem(Contexts contexts, GameRoomData roomData)
        {
            gameContext = contexts.game;
            this.roomData = roomData;
        }
        
        public void Initialize()
        {
            Console.WriteLine($"Создание игроков для игровой комнаты с номером {roomData.GameRoomNumber}");

            foreach (var playerInfo in roomData.Players)
            {
                Console.WriteLine($"Создание игрока с id = {playerInfo.PlayerGoogleId} для комнаты {roomData.GameRoomNumber}");
                
                var gameEntity = gameContext.CreateEntity();
                gameEntity.AddPlayer(playerInfo.PlayerGoogleId);
                (float x, float y) = GetRandomCoordinates();
                gameEntity.AddPosition(x, y);
            }
        }

       
        //x in [-10;10]. y in [-5;5]
        private Tuple<float, float> GetRandomCoordinates()
        {
            Random random = new Random();
            float x = 1f*random.Next(-1000, 1000)/100;
            float y = 1f*random.Next(-500, 500)/100;
            Tuple<float, float> coordinates = new Tuple<float, float>(x, y);
            return coordinates;
        }
    }
}