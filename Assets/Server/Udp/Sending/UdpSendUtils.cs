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
using Server.Udp.Storage;

//TODO запись rdup сообщений должна произвоиться даже, если ip адреса не были инициализированы
//возможно, что отправляя в первых кадрах rudp они не будут добавлены в словарь
//для того, чтобы бысто исправить это можно инициализировать ip адреса не null-ом, а рандомным ip
//(при отправке rudp всегда берётся актуальный ip)
//TODO попробовать уменьшить дублирование кода

namespace Server.Udp.Sending
{
    /// <summary>
    /// Содержит методы для отправки udp сообщений игрокам по их id.
    /// </summary>
    public class UdpSendUtils
    {
        private readonly ILog log = LogManager.GetLogger(typeof(UdpSendUtils));
        
        private readonly UdpSender udpSender;
        private readonly MatchStorage matchStorage;
        private readonly ByteArrayRudpStorage rudpStorage;

        public UdpSendUtils(MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage)
        {
            this.matchStorage = matchStorage;
            rudpStorage = byteArrayRudpStorage;
            udpSender = new UdpSender();
        }

        
        private bool TryGetPlayerIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            bool success = matchStorage.TryGetIpEndPoint(matchId, playerId, out ipEndPoint);
            // log.Warn($"{nameof(TryGetPlayerIpEndPoint)} {nameof(playerId)} {playerId} success = {success}");
            return success && ipEndPoint != null;
        }
        
        public void SendPositions(int matchId, int playerId, IEnumerable<GameEntity> withPosition)
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
                udpSender.Send(data, ipEndPoint);
            }
        }

        public void SendKill(int matchId, KillData killData)
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
                udpSender.Send(serializedMessage, ipEndPoint);
            }
        }

        public void SendReadyMadeMessage(byte[] serializedMessage, IPEndPoint ipEndPoint)
        {
            udpSender.Send(serializedMessage, ipEndPoint);
        }
        
        public void SendHealthPoints(int matchId, int targetPlayerId, float healthPoints)
        {
            int playerId = targetPlayerId;
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                HealthPointsMessage healthPointsMessage = new HealthPointsMessage(healthPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                udpSender.Send(serializedMessage, ipEndPoint);   
            }
        }
        
        public void SendMaxHealthPoints(int matchId, int playerId, float maxHealthPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                MaxHealthPointsMessage healthPointsMessage = new MaxHealthPointsMessage(maxHealthPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                udpSender.Send(serializedMessage, ipEndPoint);
            }
        }

        public void SendCooldown(int matchId, int targetPlayerId, float ability, float[] weapons)
        {
            int playerId = targetPlayerId;
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var msg = new CooldownsMessage(ability, weapons);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(msg, false, out uint messageId);
                udpSender.Send(serializedMessage, ipEndPoint);
            }
        }

        public void SendCooldownInfo(int matchId, int playerId, float abilityCooldown, WeaponInfo[] weaponsInfos)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var msg = new CooldownsInfosMessage(abilityCooldown, weaponsInfos);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(msg, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
                udpSender.Send(serializedMessage, ipEndPoint);
            }
        }

        public void SendShieldPoints(int matchId, int playerId, float shieldPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var healthPointsMessage = new ShieldPointsMessage(shieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                udpSender.Send(serializedMessage, ipEndPoint);
            }
        }

        public void SendMaxShieldPoints(int matchId, int playerId, float maxShieldPoints)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                var healthPointsMessage = new MaxShieldPointsMessage(maxShieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
                udpSender.Send(serializedMessage, ipEndPoint);
            }
        }

        public void SendShowAchievementsMessage(int matchId, int playerId)
        {
            log.Warn($"Отправка команды показать окно оезультатов боя {nameof(matchId)} {matchId} " +
                     $"{nameof(playerId)} {playerId}");
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                ShowPlayerAchievementsMessage showPlayerAchievementsMessage = new ShowPlayerAchievementsMessage(matchId);
                log.Warn($"Отправка сообщения о завершении боя игроку с id {playerId}.");
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(showPlayerAchievementsMessage, true, out uint messageId);
                rudpStorage.AddReliableMessage(matchId, playerId,  messageId, serializedMessage);
                udpSender.Send(serializedMessage, ipEndPoint);    
            }
        }

        public void SendMatchId(int matchId, int playerId)
        {
            if (TryGetPlayerIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                // Log.Warn($"Отправка сообщения о завершении боя игроку с id {PlayerId}.");
                DebugIdMessage debugIdMessage = new DebugIdMessage(matchId, playerId);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(debugIdMessage, false, out uint messageId);
                udpSender.Send(serializedMessage, ipEndPoint);
            }
        }
        
        public void SendDeliveryConfirmationMessage(DeliveryConfirmationMessage message, IPEndPoint address)
        {
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(message, false, out uint messageId);
                udpSender.Send(data, address);
            }
            else
            {
                throw new Exception("SendDeliveryConfirmationMessage address == null");
            }
        }
    }
}