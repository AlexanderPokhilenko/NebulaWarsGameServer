using Code.Common;
using Entitas;
using NetworkLibrary.NetworkLibrary;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Udp.Sending;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private static readonly Dictionary<string, SkinInfo> Skins;
        private readonly GameContext gameContext;
        private readonly BattleRoyaleMatchModel matchModel;
        private readonly UdpSendUtils udpSendUtils;
        
        private static readonly ILog Log = LogManager.CreateLogger(typeof(MapInitSystem));

        static MapInitSystem()
        {
            PlayerPrototypes = Resources.LoadAll<PlayerObject>("SO/Players")
                .ToDictionary(po => Regex.Match(po.name,
                        "^([A-Z][a-z]+)") // Первая часть до следующей большой буквы
                    .Value.ToLower());

            Skins = Resources.LoadAll<SkinInfo>("SO/Skins")
                .ToDictionary(po => Regex.Match(po.name,
                        "^([A-Z][a-z]+)") // Первая часть до следующей большой буквы
                    .Value);

            if (PlayerPrototypes.Any(p => p.Value == null))
                throw new Exception($"В {nameof(MapInitSystem)} playerPrototype был null.");

            if (Skins.Any(p => p.Value == null))
                throw new Exception($"В {nameof(MapInitSystem)} skin был null.");

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

        public MapInitSystem(Contexts contexts, BattleRoyaleMatchModel matchModel, UdpSendUtils udpSendUtils)
        {
            gameContext = contexts.game;
            this.matchModel = matchModel;
            this.udpSendUtils = udpSendUtils;
        }
        
        public void Initialize()
        {
            // Log.Info($"Создание игровой комнаты с номером {matchModel.MatchId}");
            Dictionary<int, ushort> playerInfos = new Dictionary<int, ushort>(matchModel.GameUnits.Count());
            
            var zoneEntity = FlameCircle.CreateEntity(gameContext, Vector2.zero, 0f);
            gameContext.SetZone(zoneEntity.id.value);

            var step = 360f / matchModel.GameUnits.Count();
            var halfStep = step * 0.5f;
            var offset = step / 2f;

            GameUnitsFactory factory = new GameUnitsFactory();
            List<GameUnitModel> gameUnits = factory.Create(matchModel);

            for(int gameUnitIndex = 0; gameUnitIndex < gameUnits.Count; gameUnitIndex++)
            {
                GameUnitModel gameUnit = gameUnits[gameUnitIndex];
                float angle = gameUnitIndex * step + offset;
                Vector2 position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * 40f;
                GameEntity playerEntity = PlayerPrototypes[gameUnit.WarshipName.ToLower()]
                    .CreateEntity(gameContext, position, 180f + angle, (byte)(gameUnitIndex+1));
                
                playerEntity.AddPlayer(gameUnit.TemporaryId);
                playerEntity.AddAccount(gameUnit.AccountId);
                playerInfos.Add(playerEntity.account.id, playerEntity.id.value);

                if (gameUnit.IsBot()) Match.MakeBot(playerEntity);
                if(Skins.TryGetValue(gameUnit.SkinName, out var skin)) skin.AddSkin(playerEntity, gameContext);

                var powerLevel = gameUnit.WarshipPowerLevel - 1;
                if (powerLevel > 0)
                {
                    var newHp = playerEntity.maxHealthPoints.value * (1f + powerLevel * WarshipImprovementConstants.HealthPointsCoefficient);
                    var newSpeed = playerEntity.maxVelocity.value * (1f + powerLevel * WarshipImprovementConstants.LinearVelocityCoefficient);
                    var newRotation = playerEntity.maxAngularVelocity.value * (1f + powerLevel * WarshipImprovementConstants.AngularVelocityCoefficient);
                    var attackCoefficient = 1f + powerLevel * WarshipImprovementConstants.AttackCoefficient;
                    playerEntity.ReplaceMaxHealthPoints(newHp);
                    playerEntity.ReplaceHealthPoints(newHp);
                    playerEntity.ReplaceMaxVelocity(newSpeed);
                    playerEntity.ReplaceMaxAngularVelocity(newRotation);
                    foreach (var child in playerEntity.GetAllChildrenGameEntities(gameContext))
                    {
                        child.AddAttackIncreasing(attackCoefficient);
                    }
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

                SpaceStation.CreateEntity(gameContext, wallDirection * 10f, wallAngle, 0);
                SpaceStation.CreateEntity(gameContext, wallDirection * 25f, wallAngle, 0);
                SpaceStation.CreateEntity(gameContext, wallDirection * 35f, 180f + wallAngle, 0);

                RandomBonus.CreateEntity(gameContext, wallDirection * 30f, 0);
            }

            Boss.CreateEntity(gameContext, Vector2.zero, (float) random.NextDouble() * 360f, 0);

            foreach (var playerInfo in matchModel.GameUnits.Players)
            {
                udpSendUtils.SendPlayerInfo(matchModel.MatchId, playerInfo.TemporaryId, playerInfos);
            }
        }
    }
}