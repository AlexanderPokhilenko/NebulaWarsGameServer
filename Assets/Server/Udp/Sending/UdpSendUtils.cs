using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.Udp.Storage;
using Server.Utils;

namespace Server.Udp.Sending
{
    public static class UdpSendUtils
    {
        public static void SendPositions(int targetPlayerId, IEnumerable<GameEntity> withPosition)
        {
            var gameEntities = withPosition.ToList();
            var message = new PositionsMessage
            {
                EntitiesInfo = new Dictionary<int, ViewTransform>(gameEntities.Count),
                //TODO: перенести установление в UDP с подтверждением
                PlayerEntityId = gameEntities.First(entity => entity.hasPlayer && entity.player.id == targetPlayerId)
                    .id.value
            };

            foreach (var gameEntity in gameEntities)
            {
                var gt = gameEntity.globalTransform;
                var typeId = gameEntity.viewType.id;
                var transform = new ViewTransform(gt.position, gt.angle, typeId);
                message.EntitiesInfo.Add(gameEntity.id.value, transform);
            }
            
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(message, false, out uint messageId); 
                NetworkMediator.udpBattleConnection.Send(data, address);
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
            
            HealthPointsMessage healthPointsMessage = new HealthPointsMessage(healthPoints);
            
            Log.Warning($"Отправка хп игрока {targetPlayerId} {healthPoints}");
            var serializedMessage =
                MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
            ByteArrayRudpStorage.Instance.AddMessage(targetPlayerId,  messageId, serializedMessage);
            NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
        
        }

        public static void SendBattleFinishMessage(int playerId)
        {
            var address = GetPlayerIpAddress(playerId);
            BattleFinishMessage battleFinishMessage = new BattleFinishMessage();
            
            Log.Warning($"Отправка сообщения о завершении боя игроку с id {playerId}.");
            var serializedMessage =
                MessageFactory.GetSerializedMessage(battleFinishMessage, true, out uint messageId);
            ByteArrayRudpStorage.Instance.AddMessage(playerId,  messageId, serializedMessage);
            NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
        }

        private static IPEndPoint GetPlayerIpAddress(int playerId)
        {
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(playerId) 
                          ?? throw new Exception("the address of this player is not known "+playerId);
            return address;
        }
        
    }
}