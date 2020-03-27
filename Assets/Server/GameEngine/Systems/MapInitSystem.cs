using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEngine;

namespace Server.GameEngine.Systems
{
    public class MapInitSystem : IInitializeSystem
    {
        private readonly System.Random random = new System.Random();
        private static readonly FlameCircleObject flameCircle;
        private static readonly RandomObject randomAsteroid;
        private static readonly BaseWithHealthObject spaceStation;
        private static readonly RandomObject randomBonus;
        private static readonly EntityCreatorObject boss;
        private static readonly Dictionary<string, PlayerObject> playerPrototypes;
        private readonly GameContext gameContext;
        private readonly GameRoomData roomData;
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(MapInitSystem));

        static MapInitSystem()
        {
            playerPrototypes = new Dictionary<string, PlayerObject>
            {
                {"hare", Resources.Load<PlayerObject>("SO/Players/HarePlayer")},
                {"bird", Resources.Load<PlayerObject>("SO/Players/BirdPlayer")}
            };

            if (playerPrototypes.Any(a => a.Value == null))
                throw new Exception($"В {nameof(MapInitSystem)} playerPrototype был null.");

            flameCircle = Resources.Load<FlameCircleObject>("SO/BaseObjects/FlameCircle");
            if (flameCircle == null)
                throw new Exception($"В {nameof(MapInitSystem)} flameCircle был null.");

            randomAsteroid = Resources.Load<RandomObject>("SO/BaseObjects/RandomAsteroid");
            if (randomAsteroid == null)
                throw new Exception($"В {nameof(MapInitSystem)} asteroid был null.");

            spaceStation = Resources.Load<BaseWithHealthObject>("SO/BaseObjects/SpaceStation");
            if (spaceStation == null)
                throw new Exception($"В {nameof(MapInitSystem)} spaceStation был null.");

            randomBonus = Resources.Load<RandomObject>("SO/Bonuses/PickableObjects/RandomSmallBonus");
            if (randomBonus == null)
                throw new Exception($"В {nameof(MapInitSystem)} bonus был null.");

            boss = Resources.Load<FighterObject>("SO/BaseObjects/ScarabBoss");
            if (boss == null)
                throw new Exception($"В {nameof(MapInitSystem)} boss был null.");
        }

        public MapInitSystem(Contexts contexts, GameRoomData roomData)
        {
            gameContext = contexts.game;
            this.roomData = roomData;
        }
        
        public void Initialize()
        {
            Log.Info($"Создание игровой комнаты с номером {roomData.GameRoomNumber}");
            foreach (var player in roomData.Players)
            {
                //TODO: вынести это в матчер
                if (string.IsNullOrWhiteSpace(player.WarshipName))
                {
                    player.WarshipName = playerPrototypes.ElementAt(random.Next(playerPrototypes.Count)).Key;
                    Log.Info($"Добавление боту {player.GoogleId} корабля {player.WarshipName}");
                }
            }

            var zoneEntity = flameCircle.CreateEntity(gameContext, Vector2.zero, 0f);
            gameContext.SetZone(zoneEntity.id.value);

            var step = 360f / roomData.Players.Length;
            var halfStep = step * 0.5f;
            var offset = step / 2f;

            for (var i = 0; i < roomData.Players.Length; i++)
            {
                PlayerInfoForGameRoom playerInfo = roomData.Players[i];

                var angle = i * step + offset;
                var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * 40f;
                var gameEntity = playerPrototypes[playerInfo.WarshipName].CreateEntity(gameContext, position, 180f + angle);

                gameEntity.AddPlayer(playerInfo.TemporaryId);
#warning Нужно убрать костыльную проверку на бота
                if (playerInfo.GoogleId.StartsWith("Bot_"))
                {
                    gameEntity.AddTargetingParameters(false, 13f, false);
                    gameEntity.isTargetChanging = true;
                    gameEntity.isBot = true;
                }

                var wallAngle = angle + halfStep;
                var wallDirection = CoordinatesExtensions.GetRotatedUnitVector2(wallAngle);

                for (var r = 11; r < 50; r += 25)
                {
                    for (var j = r; j < r + 10; j++)
                    {
                        randomAsteroid.CreateEntity(gameContext, wallDirection * j, (float)random.NextDouble() * 360f);
                    }
                }

                spaceStation.CreateEntity(gameContext, wallDirection * 10f, wallAngle);
                spaceStation.CreateEntity(gameContext, wallDirection * 25f, wallAngle);
                spaceStation.CreateEntity(gameContext, wallDirection * 35f, 180f + wallAngle);

                randomBonus.CreateEntity(gameContext, wallDirection * 30f, 0);
            }

            boss.CreateEntity(gameContext, Vector2.zero, (float) random.NextDouble() * 360f).isBot = true;
        }
    }
}