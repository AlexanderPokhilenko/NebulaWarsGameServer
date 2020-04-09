using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using DefaultNamespace.Libraries.NetworkLibrary.Udp.ServerToPlayer.Debug;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.GameEngine;
using Server.GameEngine.Experimental;
using Server.Udp.Storage;

namespace Server.Udp.Sending
{
    /// <summary>
    /// Содержит методы для отправки udp сообщений игрокам по их id.
    /// </summary>
    public static class UdpSendUtils
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UdpSendUtils));
        
        private static MatchStorage matchStorage;
        private static ByteArrayRudpStorage rudpStorage;

        public static void Initialize(MatchStorage matchStorageArg, ByteArrayRudpStorage rudpStorageArg)
        {
            matchStorage = matchStorageArg;
            rudpStorage = rudpStorageArg;
        }
        
        private static bool TryGetPlayerIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            return matchStorage.TryGetIpEndPoint(matchId, playerId, out ipEndPoint);
        }
        
        public static void SendPositions(int matchId, int playerId, IEnumerable<GameEntity> withPosition)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var gameEntities = withPosition.ToList();
                var message = new PositionsMessage
                {
                    //TODO что это за ужас?
                    EntitiesInfo = gameEntities.ToDictionary(e => e.id.value,
                        e => new ViewTransform(e.globalTransform.position, e.globalTransform.angle, e.viewType.id)),
                    //TODO: перенести установление в UDP с подтверждением
                    PlayerEntityId = gameEntities.First(entity => entity.hasPlayer && entity.player.id == playerId)
                        .id.value,
                    RadiusInfo = gameEntities.Where(e => e.isNonstandardRadius).ToDictionary(e => e.id.value,
                        e => e.circleCollider.radius)
                };
                var data = MessageFactory.GetSerializedMessage(message, false, out uint messageId); 
                UdpMediator.udpConnectionFacade.Send(data, ipEndPoint);
            }
        }

        internal static void SendKill(int matchId, KillData killData)
        {
            int playerId = killData.TargetPlayerId;
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var killMessage = new KillMessage(killData.KillerId, killData.KillerType, killData.VictimId, 
                    killData.VictimType);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(killMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, killData.TargetPlayerId, messageId, 
                    serializedMessage);
                UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendReadyMadeMessage(byte[] serializedMessage, IPEndPoint ipEndPoint)
        {
            UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);
        }
        
        public static void SendHealthPoints(int matchId, int targetPlayerId, float healthPoints)
        {
            int playerId = targetPlayerId;
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                HealthPointsMessage healthPointsMessage = new HealthPointsMessage(healthPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);   
            }
        }
        
        public static void SendMaxHealthPoints(int matchId, int playerId, float maxHealthPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                MaxHealthPointsMessage healthPointsMessage = new MaxHealthPointsMessage(maxHealthPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendShieldPoints(int matchId, int playerId, float shieldPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var healthPointsMessage = new ShieldPointsMessage(shieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendMaxShieldPoints(int matchId, int playerId, float maxShieldPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var healthPointsMessage = new MaxShieldPointsMessage(maxShieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
                UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendBattleFinishMessage(int matchId, int playerId)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                ShowPlayerAchievementsMessage showPlayerAchievementsMessage = new ShowPlayerAchievementsMessage(matchId);
                Log.Warn($"Отправка сообщения о завершении боя игроку с id {playerId}.");
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(showPlayerAchievementsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);    
            }
        }

        public static void SendMatchId(int matchId, int playerId)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                // Log.Warn($"Отправка сообщения о завершении боя игроку с id {PlayerId}.");
                DebugIdMessage debugIdMessage = new DebugIdMessage(matchId, playerId);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(debugIdMessage, false, out uint messageId);
                UdpMediator.udpConnectionFacade.Send(serializedMessage, ipEndPoint);
            }
        }
    }
}