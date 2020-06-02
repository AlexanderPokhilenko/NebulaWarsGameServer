using System;
using System.Collections.Generic;
using System.Linq;
using Code.Common;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Udp.Sending;
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
        private readonly UdpSendUtils udpSendUtils;
        
        private static readonly ILog Log = LogManager.CreateLogger(typeof(MapInitSystem));

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

        public MapInitSystem(Contexts contexts, BattleRoyaleMatchData matchData, UdpSendUtils udpSendUtils)
        {
            gameContext = contexts.game;
            this.matchData = matchData;
            this.udpSendUtils = udpSendUtils;
        }
        
        public void Initialize()
        {
            // Log.Info($"Создание игровой комнаты с номером {matchData.MatchId}");
            
            var zoneEntity = FlameCircle.CreateEntity(gameContext, Vector2.zero, 0f);
            gameContext.SetZone(zoneEntity.id.value);

            var step = 360f / matchData.GameUnitsForMatch.Count();
            var halfStep = step * 0.5f;
            var offset = step / 2f;

            
            for (var i = 0; i < matchData.GameUnitsForMatch.Count(); i++)
            {
                GameUnit gameUnit = matchData.GameUnitsForMatch[i];
                // UnityEngine.Debug.LogWarning($"{nameof(gameUnit.TemporaryId)} {gameUnit.TemporaryId} {nameof(gameUnit.IsBot)} {gameUnit.IsBot}");
                
                var angle = i * step + offset;
                var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * 40f;
                var gameEntity = PlayerPrototypes[gameUnit.PrefabName.ToLower()]
                    .CreateEntity(gameContext, position, 180f + angle);

                // Log.Info($"{nameof(gameUnit.TemporaryId)} {gameUnit.TemporaryId}");
                gameEntity.AddPlayer(gameUnit.TemporaryId);

                if (gameUnit.IsBot)
                {
                    Match.MakeBot(gameEntity);
                }
                else
                {
                    udpSendUtils.SendPlayerInfo(matchData.MatchId, gameUnit.TemporaryId, gameEntity.id.value);
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