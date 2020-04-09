﻿// #define USE_OLD_INIT_SYSTEMS

using System;
using System.Collections.Generic;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.GameEngine.Systems.Debug;
using Server.Udp.Sending;

//Ecs
//Ip
//RUDP
//управление состоянием

namespace Server.GameEngine
{
    public class Match
    {
        private readonly ILog log = LogManager.GetLogger(typeof(Match));

        public readonly int MatchId;
        
        private bool gameOver;
        private Contexts contexts;
        private DateTime? gameStartTime;
        private Entitas.Systems systems;
        private readonly MatchRemover matchRemover;

        public Match(int matchId, MatchRemover matchRemover)
        {
            this.matchRemover = matchRemover;
            MatchId = matchId;
        }

        public void ConfigureSystems(BattleRoyaleMatchData matchDataArg)
        {
            log.Info($"Создание нового матча {nameof(MatchId)} {MatchId}");
            
            var possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
            contexts = ContextsPool.GetContexts();
            contexts.SubscribeId();
            TryEnableDebug();
            
            systems = new Entitas.Systems()
                    // .Add(new ZoneInitSystem(Contexts, zoneObject))
                    // .Add(new PlayersInitSystem(Contexts, matchDataArg))
                    // .Add(new AsteroidsInitSystem(Contexts))
                    // .Add(new SpaceStationsInitSystem(Contexts))
                    // .Add(new BonusesInitSystem(Contexts))

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
                
                    .Add(new RudpSendingSystem(rudpMessagesStorage))
                ;

            systems.ActivateReactiveSystems();
            systems.Initialize();
            gameStartTime = DateTime.UtcNow;
        }

        public void AddPlayerExit(int playerId)
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
            systems.Execute();
            systems.Cleanup();
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
            // possibleKillersInfo.Clear();
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
        
        private void TryEnableDebug()
        {
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(contexts);
            // Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
#endif
        }

     
    }
}


/*
 
 // private readonly Dictionary<int, (int playerId, ViewTypeId type)> possibleKillersInfo;
  #region Rudp
        
        
        private IRudpMessagesStorage rudpMessagesStorage;
        private readonly ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();
        
        public void AddReliableMessage(int playerId, uint messageId, byte[] serializedMessage)
        {
            byteArrayRudpStorage.AddMessage(playerId, messageId, serializedMessage);
        }

        public bool TryRemoveRemoveRudpMessage(uint messageIdToConfirm)
        {
            return byteArrayRudpStorage.TryRemoveMessage(messageIdToConfirm);
        }

        public List<ReliableMessagesPack> GetActivePlayersRudpMessages()
        {
            List<ReliableMessagesPack> result = new List<ReliableMessagesPack>();
            
            //Для всех игроков с известными ip достать их сообщения из словаря
            foreach (var pair in ipAddressesStorage.GetPlayersRoutingData())
            {
                int playerId = pair.Key;
                IPEndPoint ipEndPoint = pair.Value;
                var messagePacks = byteArrayRudpStorage.GetAllMessagesForPlayer(playerId);
                result.Add(new ReliableMessagesPack(ipEndPoint, messagePacks));
            }
            return result;
        }
        
        #endregion
        
          #region Ip

        private readonly IpAddressesStorage ipAddressesStorage;

        public Match(int matchId, MatchRemover matchRemover)
        {
            this.matchRemover = matchRemover;

            ipAddressesStorage = new IpAddressesStorage(matchId);
            possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
        }

        public bool ContainsIpEnpPointForPlayer(int playerId)
        {
            return ipAddressesStorage.ContainsPlayerIpEndPoint(playerId);
        }

        public void AddEndPoint(int playerId, IPEndPoint ipEndPoint)
        {
            ipAddressesStorage.AddPlayer(playerId, ipEndPoint);
        }

        public bool TryGetPlayerIpEndPoint(int playerId, out IPEndPoint ipEndPoint)
        {
            return ipAddressesStorage.TryGetPlayerIpEndPoint(playerId, out ipEndPoint);
        }

      public bool TryRemovePlayerIpEndPoint(int playerId)
        {
            return ipAddressesStorage.TryRemovePlayerIp(playerId);
        }

        
        #endregion


 */