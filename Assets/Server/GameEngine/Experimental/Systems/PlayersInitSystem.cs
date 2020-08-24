// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Code.Common;
// using Entitas;
// using NetworkLibrary.NetworkLibrary.Http;
// using UnityEngine;
//
// namespace Server.GameEngine.Systems
// {
//     /// <summary>
//     /// Спавнит игроков на карте.
//     /// </summary>
//     public class PlayersInitSystem:IInitializeSystem
//     {
//         private const float Radius = 40f;
//         private readonly ServerGameContext gameContext;
//         private readonly BattleRoyaleMatchModel matchModel;
//         private static readonly Dictionary<string, PlayerObject> PlayerPrototypes;
//         
//         private static readonly ILog Log = LogManager.CreateLogger(typeof(PlayersInitSystem));
//
//         static PlayersInitSystem()
//         {
//             PlayerPrototypes = new Dictionary<string, PlayerObject>
//             {
//                 {"hare", Resources.Load<PlayerObject>("SO/Players/HarePlayer")},
//                 {"bird", Resources.Load<PlayerObject>("SO/Players/BirdPlayer")}
//             };
//
//             if (PlayerPrototypes.Any(a => a.Value == null))
//                 throw new Exception($"В {nameof(PlayersInitSystem)} asset был null.");
//         }
//
//         public PlayersInitSystem(Contexts contexts, BattleRoyaleMatchModel matchModel)
//         {
//             gameContext = contexts.serverGame;
//             this.matchModel = matchModel;
//         }
//         
//         public void Initialize()
//         {
//             Log.Info($"Создание игроков для игровой комнаты с номером {matchModel.MatchId}");
//             int startIndex = 0;
//             int finishIndex = matchModel.GameUnits.Count() - 1;
//             SpawnPlayersInACircle(matchModel.GameUnits, startIndex, finishIndex);
//             var playerModel = matchModel.GameUnits[matchModel.GameUnitsForMatch.Count() - 1]; 
//             SpawnPlayer(matchModel.GameUnitsForMatch[matchModel.GameUnitsForMatch.Count()-1], 0, 0);
//         }
//
//         /// <summary>
//         /// Стартовый индекс включается в промежуток. Последний индекс не входит в промежуток.
//         /// </summary>
//         private void SpawnPlayersInACircle(GameUnits gameUnits, int startIndex, int finishIndex)
//         {
//             int numberOfPlayersInACircle = finishIndex;
//             var step = 360f / numberOfPlayersInACircle;
//             var offset = step / 2f;
//
//             for (var i = startIndex; i < numberOfPlayersInACircle; i++)
//             {
//                 var gameUnit = gameUnits[i];
//                 var angle = i * step + offset;
//                 var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * Radius;
//                 //TODO костыль
//                 var ServerGameEntity = PlayerPrototypes[gameUnit.PrefabName.ToLower()]
//                     .CreateEntity(gameContext, position, 180f + angle, (ushort)(i+1));
//                 ServerGameEntity.AddPlayer(gameUnit.TemporaryId);
//
//                 if (gameUnit.IsBot)
//                 {
//                     ServerGameEntity.AddTargetingParameters(false, 7.5f, false);
//                     ServerGameEntity.isTargetChanging = true;
//                     ServerGameEntity.isBot = true;
//                 }
//             }
//         }
//         
//         private void SpawnPlayer(PlayerModel playerModel, int x, int y)
//         {
//             ushort teamId = (ushort) (matchModel.GameUnits.Players.Count + matchModel.GameUnits.Bots.Count); 
//             ServerGameEntity ServerGameEntity = PlayerPrototypes[playerModel.WarshipName.ToLower()]
//                 .CreateEntity(gameContext, new Vector2(x, y), 180f, teamId);
//             ServerGameEntity.AddPlayer(playerModel.TemporaryId);
//         }
//     }
// }