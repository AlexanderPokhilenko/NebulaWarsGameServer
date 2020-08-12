using System;
using System.Collections.Generic;
using Code.Common;
using Libraries.NetworkLibrary.Udp.Common;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.GameEngine;
using Server.GameEngine.Rudp;
using Server.Udp.Storage;

namespace Server.Udp.Sending
{
    public class UdpSendUtils
    {
        private readonly MessageFactory messageFactory;
        private readonly ByteArrayRudpStorage rudpStorage;
        private readonly OutgoingMessagesStorage outgoingMessagesStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(UdpSendUtils));

        public UdpSendUtils(IpAddressesStorage ipAddressesStorage, ByteArrayRudpStorage byteArrayRudpStorage, 
            OutgoingMessagesStorage outgoingMessagesStorage, MessageFactory messageFactory)
        {
            rudpStorage = byteArrayRudpStorage;
            this.messageFactory = messageFactory;
            this.outgoingMessagesStorage = outgoingMessagesStorage;
        }

        public void SendReadyMadeMessage(int matchId, ushort playerId, byte[] serializedMessage)
        {
            outgoingMessagesStorage.AddMessage(matchId, playerId, serializedMessage);
        }
        
        public void SendPlayerInfo(int matchId, ushort playerId, Dictionary<int, ushort> entityIds)
        {
            var message = new PlayerInfoMessage(entityIds);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendHides(int matchId, ushort playerId, ushort[] hiddenIds)
        {
            var message = new HidesMessage(hiddenIds);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendPositions(int matchId, ushort playerId, Dictionary<ushort, ViewTransform> entitiesInfo, bool rudp = false)
        {
            throw new NotImplementedException();
            // var length = PackingHelper.GetByteLength(entitiesInfo);
            // if (length > PackingHelper.MaxSingleMessageSize)
            // {
            //     log.Warn($"MatchId {matchId}, playerId {playerId}: превышение размера сообщения в {nameof(SendPositions)} ({length} из {PackingHelper.MaxSingleMessageSize}), выполняется разделение сообщения.");
            //     var messagesCount = (length - 1) / PackingHelper.MaxSingleMessageSize + 1;
            //     var dictionaries = entitiesInfo.Split(messagesCount);
            //     for (var i = 0; i < dictionaries.Length; i++)
            //     {
            //         var message = new PositionsMessage(dictionaries[i]);
            //         SendUdp(matchId, playerId, message, rudp);
            //     }
            // }
            // else
            // {
            //     var message = new PositionsMessage(entitiesInfo);
            //     SendUdp(matchId, playerId, message, rudp);
            // }
        }

        public void SendRadiuses(int matchId, ushort playerId, Dictionary<ushort, ushort> radiuses, bool rudp = false)
        {
            var length = PackingHelper.GetByteLength(radiuses);
            if (length > PackingHelper.MaxSingleMessageSize)
            {
                log.Warn($"MatchId {matchId}, playerId {playerId}: превышение размера сообщения в {nameof(SendRadiuses)} ({length} из {PackingHelper.MaxSingleMessageSize}), выполняется разделение сообщения.");
                var messagesCount = (length - 1) / PackingHelper.MaxSingleMessageSize + 1;
                var dictionaries = radiuses.Split(messagesCount);
                for (var i = 0; i < dictionaries.Length; i++)
                {
                    var message = new RadiusesMessage(dictionaries[i]);
                    SendUdp(matchId, playerId, message, rudp);
                }
            }
            else
            {
                var message = new RadiusesMessage(radiuses);
                SendUdp(matchId, playerId, message, rudp);
            }
        }

        public void SendParents(int matchId, ushort playerId, Dictionary<ushort, ushort> parents)
        {
            var message = new ParentsMessage(parents);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendDetaches(int matchId, ushort playerId, ushort[] detachedIds)
        {
            var message = new DetachesMessage(detachedIds);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendDestroys(int matchId, ushort playerId, ushort[] destroyedIds)
        {
            var length = PackingHelper.GetByteLength(destroyedIds);
            if (length > PackingHelper.MaxSingleMessageSize)
            {
                log.Warn($"MatchId {matchId}, playerId {playerId}: превышение размера сообщения в {nameof(SendDestroys)} ({length} из {PackingHelper.MaxSingleMessageSize}), выполняется разделение сообщения.");
                var messagesCount = (length - 1) / PackingHelper.MaxSingleMessageSize + 1;
                var arrays = destroyedIds.Split(messagesCount);
                for (var i = 0; i < arrays.Length; i++)
                {
                    var message = new DestroysMessage(arrays[i]);
                    SendUdp(matchId, playerId, message, true);
                }
            }
            else
            {
                var message = new DestroysMessage(destroyedIds);
                SendUdp(matchId, playerId, message, true);
            }
        }

        public void SendKill(int matchId, KillData killData)
        {
            var playerId = killData.TargetPlayerTmpId;
            var message = new KillMessage(killData.KillerId, killData.KillerType, killData.VictimId, killData.VictimType);
            SendUdp(matchId, playerId, message, true);
        }
        
        public void SendHealthPoints(int matchId, ushort targetPlayerId, float healthPoints)
        {
            var playerId = targetPlayerId;
            var message = new HealthPointsMessage(healthPoints);
            SendUdp(matchId, playerId, message);
        }
        
        public void SendMaxHealthPoints(int matchId, ushort playerId, float maxHealthPoints)
        {
            var message = new MaxHealthPointsMessage(maxHealthPoints);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendCooldown(int matchId, ushort targetPlayerId, float ability, float[] weapons)
        {
            var playerId = targetPlayerId;
            var message = new CooldownsMessage(ability, weapons);
            SendUdp(matchId, playerId, message);
        }

        public void SendCooldownInfo(int matchId, ushort playerId, float abilityCooldown, WeaponInfo[] weaponsInfos)
        {
            var message = new CooldownsInfosMessage(abilityCooldown, weaponsInfos);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendShieldPoints(int matchId, ushort playerId, float shieldPoints)
        {
            var message = new ShieldPointsMessage(shieldPoints);
            SendUdp(matchId, playerId, message);
        }

        public void SendMaxShieldPoints(int matchId, ushort playerId, float maxShieldPoints)
        {
            var message = new MaxShieldPointsMessage(maxShieldPoints);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendShowAchievementsMessage(int matchId, ushort playerId)
        {
            var message = new ShowPlayerAchievementsMessage(matchId);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendFrameRate(int matchId, ushort playerId, float deltaTime)
        {
            var message = new FrameRateMessage(deltaTime);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendDeliveryConfirmationMessage(int matchId, ushort playerId, uint messageNumberThatConfirms)
        {
            DeliveryConfirmationMessage mes = new DeliveryConfirmationMessage
            {
                MessageNumberThatConfirms = messageNumberThatConfirms
            };
            
            byte[] serializedMessage = messageFactory
                .GetSerializedMessage(mes, false, matchId, playerId, out uint messageId);
            outgoingMessagesStorage.AddMessage(matchId, playerId, serializedMessage);
        }
        
        private void SendUdp<T>(int matchId, ushort playerId, T message, bool rudp = false)
            where T : ITypedMessage
        {
            byte[] serializedMessage = messageFactory.GetSerializedMessage(message, rudp, matchId, playerId, out uint messageId);
            outgoingMessagesStorage.AddMessage(matchId, playerId, serializedMessage);
            if (rudp)
            {
                rudpStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
            }
        }
    }
}