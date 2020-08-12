// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Code.Common;
// using NetworkLibrary.NetworkLibrary.Http;
// using Server.GameEngine.Experimental;
// using Server.GameEngine.Experimental.Systems;
// using Server.GameEngine.Systems;
// using Server.Http;
// using Server.Udp.Sending;
// using Server.Udp.Storage;
//
// namespace Server.GameEngine.MatchLifecycle
// {
//     //TODO говно
//     //TODO нужно разбить
//     public class Match
//     {
//         private Contexts contexts;
//         public readonly int matchId;
//         private DateTime? gameStartTime;
//         // private HashSet<int> playersIds;
//         private Entitas.Systems systems;
//         private readonly MatchRemover matchRemover;
//         private PlayerDeathHandler playerDeathHandler; 
//         private readonly MatchmakerNotifier matchmakerNotifier;
//         private readonly ILog log = LogManager.CreateLogger(typeof(Match));
//
//         public Match(int matchId, MatchRemover matchRemover,
//             MatchmakerNotifier  matchmakerNotifier)
//         {
//             this.matchId = matchId;
//             this.matchRemover = matchRemover;
//             this.matchmakerNotifier = matchmakerNotifier;
//         }
//
//         //ECS
//         public void ConfigureSystems(BattleRoyaleMatchModel matchModelArg, UdpSendUtils udpSendUtils, 
//             IpAddressesStorage ipAddressesStorage)
//         {
//             log.Info($"Создание нового матча {nameof(matchId)} {matchId}");
//             
//             //TODO это нужно убрать в отдельный класс
//             Dictionary<int, (int playerId, ViewTypeId type)> possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
//
//             
//             // var test = new BattleRoyalePlayerModelFactory().Create(matchModelArg).Select(model=>model.AccountId);
//             // playersIds = new HashSet<int>(test);
//             
//             contexts = ContextsPool.GetContexts();
//             contexts.SubscribeId();
//             TryEnableDebug();
//             
//             playerDeathHandler = new PlayerDeathHandler(matchmakerNotifier, udpSendUtils, ipAddressesStorage);
//             var playersViewAreas = new PlayersViewAreas(matchModelArg.GameUnits.Players.Count);
//
//             systems = new Entitas.Systems()
//                     .Add(new MapInitSystem(contexts, matchModelArg, udpSendUtils, out var chunks))
//                     .Add(new ViewAreasInitSystem(contexts, playersViewAreas))
//                     // .Add(new TestEndMatchSystem(contexts))
//                     .Add(new PlayerMovementHandlerSystem(contexts))
//                     .Add(new PlayerAttackHandlerSystem(contexts))
//                     .Add(new PlayerAbilityHandlerSystem(contexts))
//                     .Add(new ParentsSystems(contexts))
//                     .Add(new AISystems(contexts))
//                     .Add(new MovementSystems(contexts))
//                     .Add(new GlobalTransformSystem(contexts))
//                     .Add(new ShootingSystems(contexts))
//                     .Add(new UpdatePositionChunksSystem(contexts, chunks))
//                     .Add(new CollisionSystems(contexts, chunks))
//                     .Add(new EffectsSystems(contexts))
//                     .Add(new TimeSystems(contexts))
//                     .Add(new UpdatePossibleKillersSystem(contexts, possibleKillersInfo))
//                     
//                     .Add(new PlayerExitSystem(contexts, matchModelArg.MatchId, playerDeathHandler, matchRemover))
//                     .Add(new FinishMatchSystem(contexts, matchRemover, matchId))
//                     .Add(new NetworkKillsSenderSystem(contexts, possibleKillersInfo, matchModelArg.MatchId, playerDeathHandler, udpSendUtils))
//
//                     .Add(new DestroySystems(contexts))
//                     // .Add(new MatchDebugSenderSystem(contexts, matchModelArg.MatchId, udpSendUtils))
//                     .Add(new NetworkSenderSystems(contexts, matchModelArg.MatchId, udpSendUtils, playersViewAreas))
//                     .Add(new MovingCheckerSystem(contexts))
//
//                     .Add(new DeleteSystem(contexts))
//                     .Add(new InputDeletingSystem(contexts))
//                     .Add(new GameDeletingSystem(contexts))
//                 ;
//
//             systems.ActivateReactiveSystems();
//             systems.Initialize();
//             gameStartTime = DateTime.UtcNow;
//         }
//         
//         //TODO убрать отсюда
//         public static void MakeBot(GameEntity entity)
//         {
//             entity.AddTargetingParameters(false, 13f, false);
//             entity.isTargetChanging = true;
//             entity.isBot = true;
//         }
//         
//         public void AddInputEntity<T>(ushort playerId, Action<InputEntity, T> action, T value)
//         {
//             if (contexts != null)
//             {
//                 var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
//                 if (inputEntity == null)
//                 {
//                     inputEntity = contexts.input.CreateEntity();
//                     inputEntity.AddPlayer(playerId);
//                 }
//                 action(inputEntity, value);
//             }
//         }
//
//         public void AddInputEntity(ushort playerId, Action<InputEntity> action)
//         {
//             if (contexts != null)
//             {
//                 var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
//                 if (inputEntity == null)
//                 {
//                     inputEntity = contexts.input.CreateEntity();
//                     inputEntity.AddPlayer(playerId);
//                 }
//                 action(inputEntity);
//             }
//         }
//
//         //ECS
//         public void Tick()
//         {
//             if (IsSessionTimedOut())
//             {
//                 //Тик на матче больше не будет вызван.
//                 matchRemover.MarkMatchAsFinished(matchId);
//                 return;
//             }
//             systems.Execute();
//             systems.Cleanup();
//         }
//
//         //ECS
//         public void TearDown()
//         {
//             systems.DeactivateReactiveSystems();
//             systems.TearDown();
//             systems.ClearReactiveSystems();
//             contexts.UnsubscribeId();
//             ContextsPool.RetrieveContexts(contexts);
//         }
//
//         private bool IsSessionTimedOut()
//         {
//             if (gameStartTime == null) return false;
//             var gameDuration = DateTime.UtcNow - gameStartTime.Value;
//             return gameDuration > GameSessionGlobals.MaxGameDuration;
//         }
//         
//         //ECS
//         private void TryEnableDebug()
//         {
// #if UNITY_EDITOR
//             CollidersDrawer.contextsList.Add(contexts);
//             // Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
// #endif
//         }
//     }
// }