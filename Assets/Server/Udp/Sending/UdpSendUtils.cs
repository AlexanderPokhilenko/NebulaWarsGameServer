using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DefaultNamespace.Libraries.NetworkLibrary.Udp.ServerToPlayer.Debug;
using Libraries.NetworkLibrary.Udp.Common;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.GameEngine;
using Server.Udp.Connection;
using Server.Udp.Storage;
using UnityEngine;
using ZeroFormatter;

namespace Server.Udp.Sending
{
    /// <summary>
    /// Отправляет сообщение игроку по его id.
    /// </summary>
    public class UdpSendUtils
    {
        private readonly MatchStorage matchStorage;
        private readonly ByteArrayRudpStorage rudpStorage;
        private readonly OutgoingMessagesStorage outgoingMessagesStorage;
        private readonly ILog log = LogManager.GetLogger(typeof(UdpSendUtils));

        public UdpSendUtils(MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage, 
            OutgoingMessagesStorage outgoingMessagesStorage)
        {
            this.matchStorage = matchStorage;
            rudpStorage = byteArrayRudpStorage;
            this.outgoingMessagesStorage = outgoingMessagesStorage;
        }
        
        public void SendPositions(int matchId, int playerId, IEnumerable<GameEntity> withPosition)
        {
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                List<GameEntity> gameEntities = withPosition.ToList();
                PositionsMessage message = new PositionsMessage
                {
                    //TODO что это за ужас?
                    EntitiesInfo = gameEntities.ToDictionary(e => e.id.value, e => new ViewTransform(e.globalTransform.position, e.globalTransform.angle, e.viewType.id)),
                    //TODO: перенести установление в UDP с подтверждением
                    PlayerEntityId = gameEntities.First(entity => entity.hasPlayer && entity.player.id == playerId).id.value,
                    RadiusInfo = gameEntities.Where(e => e.isNonstandardRadius).ToDictionary(e => e.id.value, e => Mathf.FloatToHalf(e.circleCollider.radius))
                };
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(message, false, out uint messageId); 
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
            else
            {
                log.Info("Отправка позиций не удалась");
            }
        }

        public void SendKill(int matchId, KillData killData)
        {
            int playerId = killData.TargetPlayerId;
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                KillMessage killMessage = new KillMessage(killData.KillerId, killData.KillerType, killData.VictimId, killData.VictimType);
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(killMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, killData.TargetPlayerId, messageId, serializedMessage);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
        }

        public void SendReadyMadeMessage(byte[] serializedMessage, IPEndPoint ipEndPoint)
        {
            outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
        }
        
        public void SendHealthPoints(int matchId, int targetPlayerId, float healthPoints)
        {
            int playerId = targetPlayerId;
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                HealthPointsMessage healthPointsMessage = new HealthPointsMessage(healthPoints);
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
        }
        
        public void SendMaxHealthPoints(int matchId, int playerId, float maxHealthPoints)
        {
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                MaxHealthPointsMessage healthPointsMessage = new MaxHealthPointsMessage(maxHealthPoints);
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
        }

        public void SendCooldown(int matchId, int targetPlayerId, float ability, float[] weapons)
        {
            int playerId = targetPlayerId;
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                CooldownsMessage msg = new CooldownsMessage(ability, weapons);
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(msg, false, out uint messageId);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
        }

        public void SendCooldownInfo(int matchId, int playerId, float abilityCooldown, WeaponInfo[] weaponsInfos)
        {
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                CooldownsInfosMessage msg = new CooldownsInfosMessage(abilityCooldown, weaponsInfos);
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(msg, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
        }

        public void SendShieldPoints(int matchId, int playerId, float shieldPoints)
        {
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                ShieldPointsMessage healthPointsMessage = new ShieldPointsMessage(shieldPoints);
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
        }

        public void SendMaxShieldPoints(int matchId, int playerId, float maxShieldPoints)
        {
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                MaxShieldPointsMessage healthPointsMessage = new MaxShieldPointsMessage(maxShieldPoints);
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }
        }

        public void SendShowAchievementsMessage(int matchId, int playerId)
        {
            log.Warn($"Отправка команды показать окно оезультатов боя {nameof(matchId)} {matchId} " +
                     $"{nameof(playerId)} {playerId}");
            if (matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                ShowPlayerAchievementsMessage showPlayerAchievementsMessage = new ShowPlayerAchievementsMessage(matchId);
                log.Warn($"Отправка сообщения о завершении боя игроку с id {playerId}.");
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(showPlayerAchievementsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);    
            }
        }
        
        public void SendDeliveryConfirmationMessage(DeliveryConfirmationMessage message, IPEndPoint playerAddress)
        {
            if (playerAddress != null)
            {
                byte[] serializedMessage = MessageFactory.GetSerializedMessage(message, false, out uint messageId);
                outgoingMessagesStorage.AddMessage(serializedMessage, playerAddress);
            }
            else
            {
                throw new Exception("SendDeliveryConfirmationMessage playerAddress == null");
            }
        }
    }
}