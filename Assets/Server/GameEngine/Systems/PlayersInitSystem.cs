using System;
using System.Linq;
using Entitas;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;

using UnityEngine;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Спавнит игроков на карте.
    /// </summary>
    public class PlayersInitSystem:IInitializeSystem
    {
        private readonly PlayerObject playerPrototype;
        private readonly GameContext gameContext;
        private readonly GameRoomData roomData;
        private const float Radius = 40f;
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayersInitSystem));

        public PlayersInitSystem(Contexts contexts, GameRoomData roomData)
        {
            gameContext = contexts.game;
            this.roomData = roomData;
            playerPrototype = Resources.Load<PlayerObject>("SO/BaseObjects/HarePlayer");
            if (playerPrototype == null)
                throw new Exception("Не удалось загрузить asset PlayerObject");
        }
        
        public void Initialize()
        {
            Log.Info($"Создание игроков для игровой комнаты с номером {roomData.GameRoomNumber}");
            SpawnPlayersInACircle(roomData.Players, 0, roomData.Players.Length - 1);
            SpawnPlayer(roomData.Players.Last(), 0, 0);
        }

        /// <summary>
        /// Стартовый индекс включается в промежуток. Последний индекс не входит в промежуток.
        /// </summary>
        private void SpawnPlayersInACircle(PlayerInfoForGameRoom[] players, int startIndex, int finishIndex)
        {
            int numberOfPlayersInACircle = finishIndex;
            var step = 360f / numberOfPlayersInACircle;
            var offset = step / 2f;

            for (var i = startIndex; i < numberOfPlayersInACircle; i++)
            {
                PlayerInfoForGameRoom playerInfo = players[i];

                var gameEntity = playerPrototype.CreateEntity(gameContext);
                var angle = i * step + offset;
                var position = Vector2.right.GetRotated(angle) * Radius;
                
                gameEntity.AddPlayer(playerInfo.TemporaryId);
                gameEntity.AddPosition(position);
                gameEntity.AddDirection(180f + angle);
            }
        }
        
        private void SpawnPlayer(PlayerInfoForGameRoom playerInfo, int x, int y)
        {
            var gameEntity = playerPrototype.CreateEntity(gameContext);
            var position = new Vector2(x,y);
            gameEntity.AddPlayer(playerInfo.TemporaryId);
            gameEntity.AddPosition(position);
            gameEntity.AddDirection(180f);
        }
    }
}