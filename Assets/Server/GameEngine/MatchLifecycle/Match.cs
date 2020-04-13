using System;
using System.Collections.Generic;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.GameEngine.Systems.Debug;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    //TODO говно
    //TODO нужно разбить
    //Ecs
    //Ip
    //управление состоянием
    public class Match
    {
        private readonly ILog log = LogManager.GetLogger(typeof(Match));

        public readonly int MatchId;
        
        private Contexts contexts;
        private DateTime? gameStartTime;
        private Entitas.Systems systems;
        private readonly MatchRemover matchRemover;
        private readonly UdpSendUtils udpSendUtils;

        private IpAddressesStorage ipAddressesStorage;

        public Match(int matchId, MatchRemover matchRemover, UdpSendUtils udpSendUtils)
        {
            MatchId = matchId;
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
        }

        public void ConfigureSystems(BattleRoyaleMatchData matchDataArg)
        {
            log.Info($"Создание нового матча {nameof(MatchId)} {MatchId}");
            
            //TODO что это за говно?
            var possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
            
            contexts = ContextsPool.GetContexts();
            contexts.SubscribeId();
            TryEnableDebug();
            
            ipAddressesStorage = new IpAddressesStorage(matchDataArg);
            
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
                    .Add(new NetworkKillsSenderSystem(contexts, possibleKillersInfo, matchDataArg.MatchId, this, udpSendUtils))
                    .Add(new PlayerExitSystem(contexts, matchDataArg.MatchId, this))
                    
                    
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

        public void AddPlayerExitEntity(int playerId)
        {
            if (contexts != null)
            {
                var inputEntity = contexts.input.CreateEntity();
                inputEntity.AddPlayerExit(playerId);
            }
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

        public List<int> GetActivePlayersIds()
        {
            return ipAddressesStorage.GetActivePlayersIds();
        }
        
        public void NotifyPlayersAboutMatchFinish()
        {
            var activePlayersIds = ipAddressesStorage.GetActivePlayersIds();
            if (activePlayersIds.Count == 0)
            {
                log.Info("Список активных игроков пуст. Некого уведомлять о окончании матча.");
            }
            else
            {
                log.Warn(" Старт уведомления игроков про окончание матча");
                foreach (int playerId in activePlayersIds)
                {
                    log.Warn($"Отправка уведомления о завершении боя игроку {nameof(playerId)} {playerId}");
                    udpSendUtils.SendBattleFinishMessage(MatchId, playerId);
                }
                log.Warn(" Конец уведомления игроков про окончание матча");
            }
        }
        
        public bool TryGetIpEndPoint(int playerId, out IPEndPoint ipEndPoint)
        {
            return ipAddressesStorage.TryGetIpEndPoint(playerId, out ipEndPoint);
        }

        public bool TryRemoveIpEndPoint(int playerId)
        {
            return ipAddressesStorage.TryRemoveIpEndPoint(playerId);
        }

        public bool HasPlayer(int playerId)
        {
            return ipAddressesStorage.HasPlayer(playerId);
        }
        
        private void TryEnableDebug()
        {
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(contexts);
            // Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
#endif
        }

        public bool TryUpdateIpEndPoint(int playerId, IPEndPoint ipEndPoint)
        {
            return ipAddressesStorage.TryUpdateIpEndPoint(playerId, ipEndPoint);
        }
    }
}