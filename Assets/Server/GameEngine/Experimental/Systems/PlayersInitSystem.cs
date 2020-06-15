using System;
using System.Collections.Generic;
using System.Linq;
using Code.Common;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEngine;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Спавнит игроков на карте.
    /// </summary>
    public class PlayersInitSystem:IInitializeSystem
    {
        private readonly System.Random random = new System.Random();
        private static readonly Dictionary<string, PlayerObject> PlayerPrototypes;
        private readonly GameContext gameContext;
        private readonly BattleRoyaleMatchData matchData;
        private const float Radius = 40f;
        
        private static readonly ILog Log = LogManager.CreateLogger(typeof(PlayersInitSystem));

        static PlayersInitSystem()
        {
            PlayerPrototypes = new Dictionary<string, PlayerObject>
            {
                {"hare", Resources.Load<PlayerObject>("SO/Players/HarePlayer")},
                {"bird", Resources.Load<PlayerObject>("SO/Players/BirdPlayer")}
            };

            if (PlayerPrototypes.Any(a => a.Value == null))
                throw new Exception($"В {nameof(PlayersInitSystem)} asset был null.");
        }

        public PlayersInitSystem(Contexts contexts, BattleRoyaleMatchData matchData)
        {
            gameContext = contexts.game;
            this.matchData = matchData;
        }
        
        public void Initialize()
        {
            Log.Info($"Создание игроков для игровой комнаты с номером {matchData.MatchId}");
            int startIndex = 0;
            int finishIndex = matchData.GameUnitsForMatch.Count() - 1;
            SpawnPlayersInACircle(matchData.GameUnitsForMatch, startIndex, finishIndex);
            SpawnPlayer(matchData.GameUnitsForMatch[matchData.GameUnitsForMatch.Count()-1], 0, 0);
        }

        /// <summary>
        /// Стартовый индекс включается в промежуток. Последний индекс не входит в промежуток.
        /// </summary>
        private void SpawnPlayersInACircle(GameUnitsForMatch gameUnits, int startIndex, int finishIndex)
        {
            int numberOfPlayersInACircle = finishIndex;
            var step = 360f / numberOfPlayersInACircle;
            var offset = step / 2f;

            for (var i = startIndex; i < numberOfPlayersInACircle; i++)
            {
                var gameUnit = gameUnits[i];
                var angle = i * step + offset;
                var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * Radius;
                //TODO костыль
                var gameEntity = PlayerPrototypes[gameUnit.PrefabName.ToLower()]
                    .CreateEntity(gameContext, position, 180f + angle, (ushort)(i+1));
                gameEntity.AddPlayer(gameUnit.TemporaryId);

                if (gameUnit.IsBot)
                {
                    gameEntity.AddTargetingParameters(false, 7.5f, false);
                    gameEntity.isTargetChanging = true;
                    gameEntity.isBot = true;
                }
            }
        }
        
        private void SpawnPlayer(GameUnit playerInfo, int x, int y)
        {
            var gameEntity = PlayerPrototypes[playerInfo.PrefabName.ToLower()]
                .CreateEntity(gameContext, new Vector2(x, y), 180f, (ushort)matchData.GameUnitsForMatch.Count());
            gameEntity.AddPlayer(playerInfo.TemporaryId);
        }
    }
}