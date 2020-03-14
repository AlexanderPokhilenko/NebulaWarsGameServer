using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;

using UnityEngine;
using Random = UnityEngine.Random;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Спавнит игроков на карте.
    /// </summary>
    public class PlayersInitSystem:IInitializeSystem
    {
        private static readonly Dictionary<string, PlayerObject> playerPrototypes;
        private readonly GameContext gameContext;
        private readonly GameRoomData roomData;
        private const float Radius = 40f;
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayersInitSystem));

        static PlayersInitSystem()
        {
            playerPrototypes = new Dictionary<string, PlayerObject>
            {
                {"hare", Resources.Load<PlayerObject>("SO/Players/HarePlayer")},
                {"bird", Resources.Load<PlayerObject>("SO/Players/BirdPlayer")}
            };

            if (playerPrototypes.Any(a => a.Value == null))
                throw new Exception($"В {nameof(PlayersInitSystem)} asset был null.");
        }

        public PlayersInitSystem(Contexts contexts, GameRoomData roomData)
        {
            gameContext = contexts.game;
            this.roomData = roomData;
        }
        
        public void Initialize()
        {
            Log.Info($"Создание игроков для игровой комнаты с номером {roomData.GameRoomNumber}");
            foreach (var player in roomData.Players)
            {
                //TODO: вынести это в матчер
                if (string.IsNullOrWhiteSpace(player.WarshipName))
                {
                    player.WarshipName = playerPrototypes.ElementAt(Random.Range(0, playerPrototypes.Count)).Key;
                    Log.Info($"Добавление боту {player.GoogleId} корабля {player.WarshipName}");
                }
            }
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

                var angle = i * step + offset;
                var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * Radius;
                var gameEntity = playerPrototypes[playerInfo.WarshipName].CreateEntity(gameContext, position, 180f + angle);

                gameEntity.AddPlayer(playerInfo.TemporaryId);
#warning Нужно убрать костыльную проверку на бота
                if (playerInfo.GoogleId.StartsWith("Bot_"))
                {
                    gameEntity.AddTargetingParameters(false, 7.5f, false);
                    gameEntity.isTargetChanging = true;
                    gameEntity.isBot = true;
                }
            }
        }
        
        private void SpawnPlayer(PlayerInfoForGameRoom playerInfo, int x, int y)
        {
            var gameEntity = playerPrototypes[playerInfo.WarshipName].CreateEntity(gameContext, new Vector2(x, y), 180f);
            gameEntity.AddPlayer(playerInfo.TemporaryId);
        }
    }
}