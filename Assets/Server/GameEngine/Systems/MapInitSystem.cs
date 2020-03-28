﻿using System;
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
        private static readonly FlameCircleObject FlameCircle;
        private static readonly RandomObject RandomAsteroid;
        private static readonly BaseWithHealthObject SpaceStation;
        private static readonly RandomObject RandomBonus;
        private static readonly EntityCreatorObject Boss;
        private static readonly Dictionary<string, PlayerObject> PlayerPrototypes;
        private readonly GameContext gameContext;
        private readonly BattleRoyaleMatchData matchData;
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(MapInitSystem));

        static MapInitSystem()
        {
            PlayerPrototypes = new Dictionary<string, PlayerObject>
            {
                {"hare", Resources.Load<PlayerObject>("SO/Players/HarePlayer")},
                {"bird", Resources.Load<PlayerObject>("SO/Players/BirdPlayer")}
            };

            if (PlayerPrototypes.Any(a => a.Value == null))
                throw new Exception($"В {nameof(MapInitSystem)} playerPrototype был null.");

            FlameCircle = Resources.Load<FlameCircleObject>("SO/BaseObjects/FlameCircle");
            if (FlameCircle == null)
                throw new Exception($"В {nameof(MapInitSystem)} flameCircle был null.");

            RandomAsteroid = Resources.Load<RandomObject>("SO/BaseObjects/RandomAsteroid");
            if (RandomAsteroid == null)
                throw new Exception($"В {nameof(MapInitSystem)} asteroid был null.");

            SpaceStation = Resources.Load<BaseWithHealthObject>("SO/BaseObjects/SpaceStation");
            if (SpaceStation == null)
                throw new Exception($"В {nameof(MapInitSystem)} spaceStation был null.");

            RandomBonus = Resources.Load<RandomObject>("SO/Bonuses/PickableObjects/RandomSmallBonus");
            if (RandomBonus == null)
                throw new Exception($"В {nameof(MapInitSystem)} bonus был null.");

            Boss = Resources.Load<FighterObject>("SO/BaseObjects/ScarabBoss");
            if (Boss == null)
                throw new Exception($"В {nameof(MapInitSystem)} boss был null.");
        }

        public MapInitSystem(Contexts contexts, BattleRoyaleMatchData matchData)
        {
            gameContext = contexts.game;
            this.matchData = matchData;
        }
        
        public void Initialize()
        {
            Log.Info($"Создание игровой комнаты с номером {matchData.MatchId}");
            
            var zoneEntity = FlameCircle.CreateEntity(gameContext, Vector2.zero, 0f);
            gameContext.SetZone(zoneEntity.id.value);

            var step = 360f / matchData.GameUnitsForMatch.Count();
            var halfStep = step * 0.5f;
            var offset = step / 2f;

            for (var i = 0; i < matchData.GameUnitsForMatch.Count(); i++)
            {
                var gameUnit = matchData.GameUnitsForMatch[i];
                
                var angle = i * step + offset;
                var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * 40f;
                var gameEntity = PlayerPrototypes[gameUnit.PrefabName.ToLower()]
                    .CreateEntity(gameContext, position, 180f + angle);

                gameEntity.AddPlayer(gameUnit.TemporaryId);

                if (gameUnit.IsBot)
                {
                    Debug.LogError("Бот");
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
                        RandomAsteroid.CreateEntity(gameContext, wallDirection * j, (float)random.NextDouble() * 360f);
                    }
                }

                SpaceStation.CreateEntity(gameContext, wallDirection * 10f, wallAngle);
                SpaceStation.CreateEntity(gameContext, wallDirection * 25f, wallAngle);
                SpaceStation.CreateEntity(gameContext, wallDirection * 35f, 180f + wallAngle);

                RandomBonus.CreateEntity(gameContext, wallDirection * 30f, 0);
            }

            Boss.CreateEntity(gameContext, Vector2.zero, (float) random.NextDouble() * 360f).isBot = true;
        }
    }
}