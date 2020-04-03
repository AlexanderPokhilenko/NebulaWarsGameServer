using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.Udp.Storage;


namespace Server.Udp.Sending
{
    public static class UdpSendUtils
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UdpSendUtils));
        public static void SendPositions(int targetPlayerId, IEnumerable<GameEntity> withPosition)
        {
            var gameEntities = withPosition.ToList();
            var message = new PositionsMessage
            {
                EntitiesInfo = gameEntities.ToDictionary(e => e.id.value,
                    e => new ViewTransform(e.globalTransform.position, e.globalTransform.angle, e.viewType.id)),
                //TODO: перенести установление в UDP с подтверждением
                PlayerEntityId = gameEntities.First(entity => entity.hasPlayer && entity.player.id == targetPlayerId)
                    .id.value,
                RadiusInfo = gameEntities.Where(e => e.isNonstandardRadius).ToDictionary(e => e.id.value,
                    e => e.circleCollider.radius)
            };
            
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(message, false, out uint messageId); 
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
        }

        internal static void SendKills(int targetPlayerId, int killerId, ViewTypeId killerType, int victimId, ViewTypeId victimType)
        {
            var address = GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var killMessage = new KillMessage(killerId, killerType, victimId, victimType);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(killMessage, true, out uint messageId);
                ByteArrayRudpStorage.Instance.AddMessage(targetPlayerId, messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
            }
        }

        public static void SendMessage(byte[] serializedMessage, int playerId)
        {
            var address = GetPlayerIpAddress(playerId);
            NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
        }
        
        public static void SendDeliveryConfirmationMessage(DeliveryConfirmationMessage message, IPEndPoint address)
        {
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(message, false, out uint messageId);
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
            else
            {
                throw new Exception("SendDeliveryConfirmationMessage address == null");
            }
        }

        public static void SendHealthPoints(int targetPlayerId, float healthPoints)
        {
            var address = GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                HealthPointsMessage healthPointsMessage = new HealthPointsMessage(healthPoints);
                // Log.Warning($"Отправка хп игрока {targetPlayerId} {healthPoints}");
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                // ByteArrayRudpStorage.Instance.AddMessage(targetPlayerId,  messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);   
            }
        }
        
        public static void SendMaxHealthPoints(int targetPlayerId, float maxHealthPoints)
        {
            var address = GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                MaxHealthPointsMessage healthPointsMessage = new MaxHealthPointsMessage(maxHealthPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                ByteArrayRudpStorage.Instance.AddMessage(targetPlayerId,  messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
            }
        }

        public static void SendShieldPoints(int targetPlayerId, float shieldPoints)
        {
            var address = GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var healthPointsMessage = new ShieldPointsMessage(shieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, false, out uint messageId);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
            }
        }

        public static void SendMaxShieldPoints(int targetPlayerId, float maxShieldPoints)
        {
            var address = GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var healthPointsMessage = new MaxShieldPointsMessage(maxShieldPoints);
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                ByteArrayRudpStorage.Instance.AddMessage(targetPlayerId, messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
            }
        }

        public static void SendBattleFinishMessage(int playerId)
        {
            var address = GetPlayerIpAddress(playerId);
            ShowPlayerAchievementsMessage showPlayerAchievementsMessage = new ShowPlayerAchievementsMessage();
            if (address != null)
            {
                Log.Warn($"Отправка сообщения о завершении боя игроку с id {playerId}.");
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(showPlayerAchievementsMessage, true, out uint messageId);
                ByteArrayRudpStorage.Instance.AddMessage(playerId,  messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);    
            }
        }

        private static IPEndPoint GetPlayerIpAddress(int playerId)
        {
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(playerId);
            if (address == null)
            {
                Log.Warn($"Не найден ip для игрока {playerId}");
            }
            return address;
        }
        
    }
}