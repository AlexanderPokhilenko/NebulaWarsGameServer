using System;
using System.Collections.Generic;
using System.Linq;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    //TODO говно
    //TODO нужно разбить
    public class Match
    {
        private Contexts contexts;
        public readonly int MatchId;
        private DateTime? gameStartTime;
        private HashSet<int> playersIds;
        private Entitas.Systems systems;
        private readonly MatchRemover matchRemover;
        private PlayerDeathHandler playerDeathHandler; 
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly ILog log = LogManager.CreateLogger(typeof(Match));

        public Match(int matchId, MatchRemover matchRemover,
            MatchmakerNotifier  matchmakerNotifier)
        {
            MatchId = matchId;
            this.matchRemover = matchRemover;
            this.matchmakerNotifier = matchmakerNotifier;
        }

        //ECS
        public void ConfigureSystems(BattleRoyaleMatchData matchDataArg, UdpSendUtils udpSendUtils)
        {
            log.Info($"Создание нового матча {nameof(MatchId)} {MatchId}");
            
            //TODO это нужно убрать в отдельный класс
            var possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
            
            playersIds = new HashSet<int>(matchDataArg.GameUnitsForMatch
                .Select(gu=>(int)gu.TemporaryId));
            
            contexts = ContextsPool.GetContexts();
            contexts.SubscribeId();
            TryEnableDebug();
            
            playerDeathHandler = new PlayerDeathHandler(matchmakerNotifier, udpSendUtils);
            
            systems = new Entitas.Systems()
                    
                    .Add(new MapInitSystem(contexts, matchDataArg, udpSendUtils))
                    // .Add(new TestEndMatchSystem(contexts))
                    .Add(new PlayerMovementHandlerSystem(contexts))
                    .Add(new PlayerAttackHandlerSystem(contexts))
                    .Add(new PlayerAbilityHandlerSystem(contexts))
                    .Add(new ParentsSystems(contexts))
                    .Add(new AISystems(contexts))
                    .Add(new MovementSystems(contexts))
                    .Add(new GlobalTransformSystem(contexts))
                    .Add(new ShootingSystems(contexts))
                    .Add(new CollisionSystems(contexts))
                    .Add(new EffectsSystems(contexts))
                    .Add(new TimeSystems(contexts))
                    .Add(new UpdatePossibleKillersSystem(contexts, possibleKillersInfo))
                    
                    .Add(new PlayerExitSystem(contexts, matchDataArg.MatchId, playerDeathHandler))
                    .Add(new FinishMatchSystem(contexts, matchRemover, MatchId))
                    .Add(new NetworkKillsSenderSystem(contexts, possibleKillersInfo, matchDataArg.MatchId, playerDeathHandler, udpSendUtils))
                    
                    .Add(new DestroySystems(contexts))
                    // .Add(new MatchDebugSenderSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new PositionsSenderSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new HealthUpdaterSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new MaxHpUpdaterSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new CooldownInfoUpdaterSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new CooldownUpdaterSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new ShieldPointsUpdaterSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new InputDeletingSystem(contexts))
                    .Add(new GameDeletingSystem(contexts))
                ;

            systems.ActivateReactiveSystems();
            systems.Initialize();
            gameStartTime = DateTime.UtcNow;
        }
        
        //TODO убрать отсюда
        public static void MakeBot(GameEntity entity)
        {
            entity.AddTargetingParameters(false, 13f, false);
            entity.isTargetChanging = true;
            entity.isBot = true;
        }
        
        public void AddInputEntity<T>(int playerId, Action<InputEntity, T> action, T value)
        {
            if (contexts != null)
            {
                var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
                if (inputEntity == null)
                {
                    inputEntity = contexts.input.CreateEntity();
                    inputEntity.AddPlayer(playerId);
                }
                action(inputEntity, value);
            }
        }

        public void AddInputEntity(int playerId, Action<InputEntity> action)
        {
            if (contexts != null)
            {
                var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
                if (inputEntity == null)
                {
                    inputEntity = contexts.input.CreateEntity();
                    inputEntity.AddPlayer(playerId);
                }
                action(inputEntity);
            }
        }

        //ECS
        public void Tick()
        {
            if (IsSessionTimedOut())
            {
                //Тик на матче больше не будет вызван.
                matchRemover.MarkMatchAsFinished(MatchId);
                return;
            }
            systems.Execute();
            systems.Cleanup();
        }

        //ECS
        public void TearDown()
        {
            systems.DeactivateReactiveSystems();
            systems.TearDown();
            systems.ClearReactiveSystems();
            contexts.UnsubscribeId();
            ContextsPool.RetrieveContexts(contexts);
        }

        private bool IsSessionTimedOut()
        {
            if (gameStartTime == null) return false;
            var gameDuration = DateTime.UtcNow - gameStartTime.Value;
            return gameDuration > GameSessionGlobals.MaxGameDuration;
        }
        
        //ECS
        private void TryEnableDebug()
        {
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(contexts);
            // Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
#endif
        }

        public bool HasPlayer(int playerId)
        {
            return playersIds.Contains(playerId);
        }
    }
}