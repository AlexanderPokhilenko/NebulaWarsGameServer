using System;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Utils;
using UnityEditor;
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

            var step = 360f / roomData.Players.Length;
            var offset = step / 2f;

            for (var i = 0; i < roomData.Players.Length; i++)
            {
                var playerInfo = roomData.Players[i];
                Log.Info($"Создание игрока с id = {playerInfo.GoogleId} для комнаты {roomData.GameRoomNumber}");

                var gameEntity = playerPrototype.CreateEntity(gameContext);
                gameEntity.AddPlayer(playerInfo.TemporaryId);

                var angle = i * step + offset;
                var position = Vector2.right.GetRotated(angle) * Radius;

                gameEntity.AddPosition(position);
                gameEntity.AddDirection(180f + angle);
            }
        }
    }
}