﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.GameEngine;

//TODO говно

namespace Server.Udp.Sending
{
    public static class UdpSendUtils
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UdpSendUtils));

        private static bool TryGetPlayerIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            return GameEngineMediator.MatchStorageFacade.TryGetPlayerIpEndPoint(matchId, playerId, out ipEndPoint);
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
                NetworkMediator.udpBattleConnection.Send(data, ipEndPoint);
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
                GameEngineMediator.MatchStorageFacade.AddReliableMessage(matchId, killData.TargetPlayerId, messageId, 
                    serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendReadyMadeMessage(byte[] serializedMessage, IPEndPoint ipEndPoint)
        {
            NetworkMediator.udpBattleConnection.Send(serializedMessage, ipEndPoint);
        }
        
        public static void SendHealthPoints(int matchId, int targetPlayerId, float healthPoints)
        {
            int playerId = targetPlayerId;
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                HealthPointsMessage healthPointsMessage = new HealthPointsMessage(healthPoints);
                // Log.Warning($"Отправка хп игрока {TargetPlayerId} {healthPoints}");
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                // ByteArrayRudpStorage.Instance.AddMessage(TargetPlayerId,  messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, ipEndPoint);   
            }
        }
        
        public static void SendMaxHealthPoints(int matchId, int playerId, float maxHealthPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                MaxHealthPointsMessage healthPointsMessage = new MaxHealthPointsMessage(maxHealthPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                GameEngineMediator.MatchStorageFacade.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendShieldPoints(int matchId, int playerId, float shieldPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var healthPointsMessage = new ShieldPointsMessage(shieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendMaxShieldPoints(int matchId, int playerId, float maxShieldPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var healthPointsMessage = new MaxShieldPointsMessage(maxShieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                GameEngineMediator.MatchStorageFacade.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, ipEndPoint);
            }
        }

        public static void SendBattleFinishMessage(int matchId, int playerId)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                ShowPlayerAchievementsMessage showPlayerAchievementsMessage = new ShowPlayerAchievementsMessage();
                Log.Warn($"Отправка сообщения о завершении боя игроку с id {playerId}.");
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(showPlayerAchievementsMessage, true, out uint messageId);
                GameEngineMediator.MatchStorageFacade.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, ipEndPoint);    
            }
        }
    }
}