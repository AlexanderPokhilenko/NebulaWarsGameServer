// #define USE_OLD_INIT_SYSTEMS

using System;
using System.Collections.Generic;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Experimental;
using Server.GameEngine.Systems;
using Server.GameEngine.Systems.Debug;
using Server.Udp.Storage;

//TODO говно
//разделить на подклассы
//Ecs
//Ip
//RUDP
//управление состоянием


namespace Server.GameEngine
{
    public class Match
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Match));
        
        #region Ip

        private readonly IpAddressesStorage ipAddressesStorage;

        public Match(MatchStorageFacade matchStorageFacade, int matchId)
        {
            ipAddressesStorage = new IpAddressesStorage(matchId);
            
            this.matchStorageFacade = matchStorageFacade;
            possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
            //TODO: как-то обойтись без использования AssetDatabase; добавить возможность менять параметры зоны для разных карт
            // zoneObject = Resources.Load<FlameCircleObject>("SO/BaseObjects/FlameCircle");
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

     
        
        #endregion

        #region Rudp
        
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
        
        #region Ecs
        private Entitas.Systems systems;
        public Contexts Contexts { get; private set; }
       
        
        public void ConfigureSystems(BattleRoyaleMatchData matchDataArg)
        {
            Log.Info("Создание новой комнаты номер = "+matchDataArg.MatchId);

            matchData = matchDataArg;
            Contexts = ContextsPool.GetContexts();
            Contexts.SubscribeId();
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(Contexts);
            // Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
#endif
            systems = new Entitas.Systems()
#if USE_OLD_INIT_SYSTEMS
                    .Add(new ZoneInitSystem(Contexts, zoneObject))
                    .Add(new PlayersInitSystem(Contexts, matchDataArg))
                    .Add(new AsteroidsInitSystem(Contexts))
                    .Add(new SpaceStationsInitSystem(Contexts))
                    .Add(new BonusesInitSystem(Contexts))
#else
                    .Add(new MapInitSystem(Contexts, matchDataArg))
#endif
                    .Add(new MatchIdInitSystem(Contexts, matchDataArg))
                    
                    // .Add(new TestEndMatchSystem2(Contexts))
                    
                    
                    .Add(new PlayerMovementHandlerSystem(Contexts))
                    .Add(new PlayerAttackHandlerSystem(Contexts))
                    .Add(new ParentsSystems(Contexts))
                    .Add(new AISystems(Contexts))
                    .Add(new MovementSystems(Contexts))
                    .Add(new GlobalTransformSystem(Contexts))
                    .Add(new ShootingSystems(Contexts))
                    .Add(new CollisionSystems(Contexts))
                    .Add(new EffectsSystems(Contexts))
                    .Add(new TimeSystems(Contexts))
                    .Add(new UpdatePossibleKillersSystem(Contexts, possibleKillersInfo))
                    
                    
                    .Add(new FinishMatchSystem(Contexts, this))
                    .Add(new NetworkKillsSenderSystem(Contexts, possibleKillersInfo, matchDataArg.MatchId))
                    .Add(new PlayerExitSystem(Contexts, matchDataArg.MatchId, matchStorageFacade))
                    
                    
                    
                    
                    .Add(new DestroySystems(Contexts))
                    .Add(new MatchDebugSenderSystem(Contexts, matchDataArg.MatchId))
                    .Add(new NetworkSenderSystem(Contexts, matchDataArg.MatchId))
                    .Add(new MaxHpUpdaterSystem(Contexts, matchDataArg.MatchId))
                    .Add(new ShieldPointsUpdaterSystem(Contexts, matchDataArg.MatchId))
                    .Add(new InputDeletingSystem(Contexts))
                    .Add(new GameDeletingSystem(Contexts))
                ;

            systems.ActivateReactiveSystems();
            systems.Initialize();
            gameStartTime = DateTime.UtcNow;
        }
        
        public void Execute()
        {
            if (gameOver) return;
            if (IsSessionTimedOut())
            {
                FinishGame();
                return;
            }
            systems.Execute();
        }

        public void Cleanup()
        {
            if (gameOver) return;
            systems.Cleanup();
        }

        public void TearDown()
        {
            systems.DeactivateReactiveSystems();
            systems.TearDown();
            systems.ClearReactiveSystems();
            Contexts.UnsubscribeId();
            ContextsPool.RetrieveContexts(Contexts);
            possibleKillersInfo.Clear();
        }
        
        #endregion

        #region Dich
        private DateTime? gameStartTime;
        private readonly MatchStorageFacade matchStorageFacade;
        public BattleRoyaleMatchData matchData { get; private set; }
        private readonly Dictionary<int, (int playerId, ViewTypeId type)> possibleKillersInfo;
        private bool gameOver;
        #endregion

      
        
        private bool IsSessionTimedOut()
        {
            if (gameStartTime == null) return false;
            var gameDuration = DateTime.UtcNow - gameStartTime.Value;
            return gameDuration > GameSessionGlobals.GameDuration;
        }

        public void FinishGame()
        {
            gameOver = true;
            matchStorageFacade.MarkBattleAsFinished(matchData.MatchId);
        }

        public bool TryRemovePlayerIpEndPoint(int playerId)
        {
            return ipAddressesStorage.TryRemovePlayerIp(playerId);
        }
    }
}