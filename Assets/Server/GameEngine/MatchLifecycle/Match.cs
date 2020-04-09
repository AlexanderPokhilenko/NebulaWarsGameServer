using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.GameEngine.Systems.Debug;
using Server.Udp.Sending;
using Server.Udp.Storage;

//Ecs
//Ip
//RUDP
//управление состоянием

namespace Server.GameEngine
{
    //TODO нужно разбить
    public class Match
    {
        private readonly ILog log = LogManager.GetLogger(typeof(Match));

        public readonly int MatchId;
        
        private bool gameOver;
        private Contexts contexts;
        private DateTime? gameStartTime;
        private Entitas.Systems systems;
        private readonly MatchRemover matchRemover;

        private readonly IpAddressesStorage ipAddressesStorage;
        
        public Match(int matchId, MatchRemover matchRemover)
        {
            MatchId = matchId;
            this.matchRemover = matchRemover;
            ipAddressesStorage = new IpAddressesStorage(matchId);
        }

        public void ConfigureSystems(BattleRoyaleMatchData matchDataArg)
        {
            log.Info($"Создание нового матча {nameof(MatchId)} {MatchId}");
            
            var possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
            contexts = ContextsPool.GetContexts();
            contexts.SubscribeId();
            TryEnableDebug();
            
            systems = new Entitas.Systems()
                    
                    .Add(new MapInitSystem(contexts, matchDataArg))
                    .Add(new MatchIdInitSystem(contexts, matchDataArg))
                    // .Add(new TestEndMatchSystem2(Contexts))
                    .Add(new PlayerMovementHandlerSystem(contexts))
                    .Add(new PlayerAttackHandlerSystem(contexts))
                    .Add(new ParentsSystems(contexts))
                    .Add(new AISystems(contexts))
                    .Add(new MovementSystems(contexts))
                    .Add(new GlobalTransformSystem(contexts))
                    .Add(new ShootingSystems(contexts))
                    .Add(new CollisionSystems(contexts))
                    .Add(new EffectsSystems(contexts))
                    .Add(new TimeSystems(contexts))
                    .Add(new UpdatePossibleKillersSystem(contexts, possibleKillersInfo))
                    
                    
                    .Add(new FinishMatchSystem(contexts, this))
                    .Add(new NetworkKillsSenderSystem(contexts, possibleKillersInfo, matchDataArg.MatchId))
                    .Add(new PlayerExitSystem(contexts, matchDataArg.MatchId, matchStorageFacade))
                    
                    
                    .Add(new DestroySystems(contexts))
                    .Add(new MatchDebugSenderSystem(contexts, matchDataArg.MatchId))
                    .Add(new NetworkSenderSystem(contexts, matchDataArg.MatchId))
                    .Add(new MaxHpUpdaterSystem(contexts, matchDataArg.MatchId))
                    .Add(new ShieldPointsUpdaterSystem(contexts, matchDataArg.MatchId))
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
            //TODO опасно
            Execute();
            Cleanup();
        }
        
        private void Execute()
        {
            if (gameOver) return;
            if (IsSessionTimedOut())
            {
                Finish();
                return;
            }
            systems.Execute();
        }

        private void Cleanup()
        {
            if (gameOver) return;
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
            return gameDuration > GameSessionGlobals.GameDuration;
        }

        //TODO плохо
        public void Finish()
        {
            gameOver = true;
            matchRemover.MarkMatchAsFinished(MatchId);
        }
       
        public void NotifyPlayersAboutMatchFinish()
        {
            log.Warn($" Старт уведомления игроков про окончание матча");
            foreach (int playerId in playersIds)
            {
                log.Warn($"Отправка уведомления о завуршении боя игроку {nameof(playerId)} {playerId}");
                UdpSendUtils.SendBattleFinishMessage(MatchId, playerId);
            }
            log.Warn($" Конец уведомления игроков про окончание матча");
            throw new NotImplementedException();
        }

        public bool HasIpEnpPoint(int playerId)
        {
            return ipAddressesStorage.ContainsPlayerIpEndPoint(playerId);
        }

        public void AddIpEndPoint(int playerId, IPEndPoint ipEndPoint)
        {
            ipAddressesStorage.AddPlayer(playerId, ipEndPoint);
        }

        public bool TryGetIpEndPoint(int playerId, out IPEndPoint ipEndPoint)
        {
            return ipAddressesStorage.TryGetPlayerIpEndPoint(playerId, out ipEndPoint);
        }

        public bool TryRemoveIpEndPoint(int playerId)
        {
            return ipAddressesStorage.TryRemovePlayerIp(playerId);
        }

        private void TryEnableDebug()
        {
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(contexts);
            // Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
#endif
        }

        public void PingTryAddIpEndPoint(int playerId, IPEndPoint ipEndPoint)
        {
            //этот игрок уже есть в таблице ip адресов?
            //этот игрок есть в matchData?
            //этого игрока не удаляли?
            //добавить ip
            throw new NotImplementedException();
        }
    }
}