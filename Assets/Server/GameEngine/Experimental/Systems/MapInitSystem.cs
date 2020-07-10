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
        private readonly BattleRoyaleMatchModel matchModel;
        private readonly UdpSendUtils udpSendUtils;
        
        private static readonly ILog Log = LogManager.CreateLogger(typeof(MapInitSystem));

        static MapInitSystem()
        {
            PlayerPrototypes = new Dictionary<string, PlayerObject>
            {
                {"hare", Resources.Load<PlayerObject>("SO/Players/HarePlayer")},
                {"bird", Resources.Load<PlayerObject>("SO/Players/BirdPlayer")},
                {"smiley", Resources.Load<PlayerObject>("SO/Players/SmileyPlayer")}
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

            BattleRoyalePlayerModelFactory factory = new BattleRoyalePlayerModelFactory();

            BattleRoyalePlayerModel[] gameUnits = factory.Create(matchModel);

            for(int gameUnitIndex = 0; gameUnitIndex < gameUnits.Length; gameUnitIndex++)
            {
                BattleRoyalePlayerModel gameUnit = gameUnits[gameUnitIndex];
                float angle = gameUnitIndex * step + offset;
                Vector2 position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * 40f;
                GameEntity playerEntity = PlayerPrototypes[gameUnit.WarshipName.ToLower()]
                    .CreateEntity(gameContext, position, 180f + angle, (ushort)(gameUnitIndex+1));
                
                playerEntity.AddPlayer((ushort) gameUnit.AccountId);

                //TODO: улучшать по отдельным параметрам
                float newHp = playerEntity.maxHealthPoints.value * (1f + gameUnit.WarshipPowerLevel * 0.075f);
                float newSpeed = playerEntity.maxVelocity.value * (1f + gameUnit.WarshipPowerLevel * 0.025f);
                float newRotation = playerEntity.maxAngularVelocity.value * (1f + gameUnit.WarshipPowerLevel * 0.025f);
                float attackCoefficient = 1f + gameUnit.WarshipPowerLevel * 0.05f;
                playerEntity.ReplaceMaxHealthPoints(newHp);
                playerEntity.ReplaceHealthPoints(newHp);
                playerEntity.ReplaceMaxVelocity(newSpeed);
                playerEntity.ReplaceMaxAngularVelocity(newRotation);
                foreach (var child in playerEntity.GetAllChildrenGameEntities(gameContext))
                {
                    child.AddAttackIncreasing(attackCoefficient);
                }

                if (gameUnit.IsBot())
                {
                    playerEntity.AddAccount(gameUnit.AccountId);
                    Match.MakeBot(playerEntity);
                }
                else
                {
                    playerEntity.AddAccount(gameUnit.AccountId);
                }
                playerInfos.Add(playerEntity.account.id, playerEntity.id.value);

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
            
            // for (int gameUnitIndex = 0; gameUnitIndex < matchModel.GameUnits.Count(); gameUnitIndex++)
            // {
            //     GameUnit gameUnit = matchModel.GameUnitsForMatch[gameUnitIndex];
            //     
            //     var angle = gameUnitIndex * step + offset;
            //     var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * 40f;
            //     var playerEntity = PlayerPrototypes[gameUnit.PrefabName.ToLower()]
            //         .CreateEntity(gameContext, position, 180f + angle, (ushort)(gameUnitIndex+1));
            //
            //     // Log.Info($"{nameof(gameUnit.TemporaryId)} {gameUnit.TemporaryId}");
            //     playerEntity.AddPlayer(gameUnit.TemporaryId);
            //
            //     //TODO: улучшать по отдельным параметрам
            //     var newHp = playerEntity.maxHealthPoints.value * (1f + gameUnit.WarshipCombatPowerLevel * 0.075f);
            //     var newSpeed = playerEntity.maxVelocity.value * (1f + gameUnit.WarshipCombatPowerLevel * 0.025f);
            //     var newRotation = playerEntity.maxAngularVelocity.value * (1f + gameUnit.WarshipCombatPowerLevel * 0.025f);
            //     var attackCoefficient = 1f + gameUnit.WarshipCombatPowerLevel * 0.05f;
            //     playerEntity.ReplaceMaxHealthPoints(newHp);
            //     playerEntity.ReplaceHealthPoints(newHp);
            //     playerEntity.ReplaceMaxVelocity(newSpeed);
            //     playerEntity.ReplaceMaxAngularVelocity(newRotation);
            //     foreach (var child in playerEntity.GetAllChildrenGameEntities(gameContext))
            //     {
            //         child.AddAttackIncreasing(attackCoefficient);
            //     }
            //
            //     if (gameUnit.IsBot)
            //     {
            //         playerEntity.AddAccount(-((BotInfo)gameUnit).TemporaryId);
            //         Match.MakeBot(playerEntity);
            //     }
            //     else
            //     {
            //         playerEntity.AddAccount(((PlayerInfoForMatch)gameUnit).AccountId);
            //     }
            //     playerInfos.Add(playerEntity.account.id, playerEntity.id.value);
            //
            //     var wallAngle = angle + halfStep;
            //     var wallDirection = CoordinatesExtensions.GetRotatedUnitVector2(wallAngle);
            //
            //     for (var r = 11; r < 50; r += 25)
            //     {
            //         for (var j = r; j < r + 10; j++)
            //         {
            //             RandomAsteroid.CreateEntity(gameContext, wallDirection * j, (float)random.NextDouble() * 360f);
            //         }
            //     }
            //
            //     SpaceStation.CreateEntity(gameContext, wallDirection * 10f, wallAngle, 0);
            //     SpaceStation.CreateEntity(gameContext, wallDirection * 25f, wallAngle, 0);
            //     SpaceStation.CreateEntity(gameContext, wallDirection * 35f, 180f + wallAngle, 0);
            //
            //     RandomBonus.CreateEntity(gameContext, wallDirection * 30f, 0);
            // }

            Boss.CreateEntity(gameContext, Vector2.zero, (float) random.NextDouble() * 360f, 0);

            foreach (var playerInfo in matchModel.GameUnits.Players)
            {
                udpSendUtils.SendPlayerInfo(matchModel.MatchId, playerInfo.TemporaryId, playerInfos);
            }
        }
    }
}