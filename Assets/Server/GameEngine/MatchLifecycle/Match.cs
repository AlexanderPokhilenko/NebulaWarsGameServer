using System;
using System.Collections.Generic;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.GameEngine.Systems.Debug;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    //TODO говно
    //TODO нужно разбить
    //Ecs
    //Ip
    public class Match
    {
        private readonly ILog log = LogManager.GetLogger(typeof(Match));

        public readonly int MatchId;
        
        private Contexts contexts;
        private DateTime? gameStartTime;
        private Entitas.Systems systems;
        private readonly MatchRemover matchRemover;
        private readonly MatchmakerMatchStatusNotifier matchmakerMatchStatusNotifier;

        private IpAddressesStorage ipAddressesStorage;
        private PlayerDeathHandler playerDeathHandler; 

        public Match(int matchId, MatchRemover matchRemover,
            MatchmakerMatchStatusNotifier  matchmakerMatchStatusNotifier)
        {
            MatchId = matchId;
            this.matchRemover = matchRemover;
            this.matchmakerMatchStatusNotifier = matchmakerMatchStatusNotifier;
        }

        //ECS
        public void ConfigureSystems(BattleRoyaleMatchData matchDataArg, UdpSendUtils udpSendUtils)
        {
            log.Info($"Создание нового матча {nameof(MatchId)} {MatchId}");
            
            //TODO что это за говно?
            var possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
            
            contexts = ContextsPool.GetContexts();
            contexts.SubscribeId();
            TryEnableDebug();
            // throw new NotImplementedException();
            
            ipAddressesStorage = new IpAddressesStorage(matchDataArg);
            playerDeathHandler = new PlayerDeathHandler(ipAddressesStorage, matchmakerMatchStatusNotifier, udpSendUtils);
            
            systems = new Entitas.Systems()
                    
                    .Add(new MapInitSystem(contexts, matchDataArg))
                    // .Add(new TestEndMatchSystem2(Contexts))
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
                    
                    
                    .Add(new FinishMatchSystem(contexts, matchRemover, MatchId))
                    .Add(new NetworkKillsSenderSystem(contexts, possibleKillersInfo, matchDataArg.MatchId, playerDeathHandler, udpSendUtils))
                    .Add(new PlayerExitSystem(contexts, matchDataArg.MatchId, playerDeathHandler))
                    
                    
                    .Add(new DestroySystems(contexts))
                    .Add(new MatchDebugSenderSystem(contexts, matchDataArg.MatchId, udpSendUtils))
                    .Add(new NetworkSenderSystem(contexts, matchDataArg.MatchId, udpSendUtils))
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

        //ECS
        public void AddPlayerExitEntity(int playerId)
        {
            if (contexts != null)
            {
                var inputEntity = contexts.input.CreateEntity();
                // inputEntity.AddPlayerExit(playerId);
            }
        }
        
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

        //Ip
        public List<int> GetActivePlayersIds()
        {
            return ipAddressesStorage.GetActivePlayersIds();
        }

        //Ip
        public bool TryGetIpEndPoint(int playerId, out IPEndPoint ipEndPoint)
        {
            return ipAddressesStorage.TryGetIpEndPoint(playerId, out ipEndPoint);
        }
        
        //Ip
        // public bool TryRemoveIpEndPoint(int playerId)
        // {
        //     return ipAddressesStorage.TryRemoveIpEndPoint(playerId);
        // }

        //Ip
        public bool HasPlayer(int playerId)
        {
            return ipAddressesStorage.HasPlayer(playerId);
        }
        
        //ECS
        private void TryEnableDebug()
        {
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(contexts);
            // Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
#endif
        }

        //Ip
        public bool TryUpdateIpEndPoint(int playerId, IPEndPoint ipEndPoint)
        {
            return ipAddressesStorage.TryUpdateIpEndPoint(playerId, ipEndPoint);
        }
    }
}