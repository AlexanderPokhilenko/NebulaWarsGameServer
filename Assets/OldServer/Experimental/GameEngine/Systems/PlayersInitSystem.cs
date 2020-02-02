using System;
using System.Collections.Concurrent;
using System.Threading;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEngine;

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
                var rndPosition = GetRandomCoordinates();
                gameEntity.AddPosition(rndPosition);
            }
        }

       
        //x in [-10;10]. y in [-5;5]
        private Vector2 GetRandomCoordinates()
        {
            //var random = new System.Random();
            float x = /*random.Next(-1000, 1000)/100f;*/ UnityEngine.Random.Range(-10f, 10f);
            float y = /*random.Next(-500, 500)/100f;*/ UnityEngine.Random.Range(-5f, 5f);
            var coordinates = new Vector2(x, y);
            return coordinates;
        }
    }
}