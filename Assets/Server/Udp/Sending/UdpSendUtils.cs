﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Code.Common;
using Libraries.NetworkLibrary.Udp;
using Libraries.NetworkLibrary.Udp.Common;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.GameEngine;
using Server.Udp.Storage;
using UnityEngine;

namespace Server.Udp.Sending
{
    /// <summary>
    /// Отправляет сообщение игроку по его id.
    /// </summary>
    public class UdpSendUtils
    {
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly ByteArrayRudpStorage rudpStorage;
        private readonly OutgoingMessagesStorage outgoingMessagesStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(UdpSendUtils));

        public UdpSendUtils(IpAddressesStorage ipAddressesStorage, ByteArrayRudpStorage byteArrayRudpStorage, 
            OutgoingMessagesStorage outgoingMessagesStorage)
        {
            this.ipAddressesStorage = ipAddressesStorage;
            rudpStorage = byteArrayRudpStorage;
            this.outgoingMessagesStorage = outgoingMessagesStorage;
        }

        public void SendReadyMadeMessage(byte[] serializedMessage, IPEndPoint ipEndPoint)
        {
            outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
        }

        private void SendUdp<T>(int matchId, int playerId, T message, bool rudp = false) where T : ITypedMessage
        {
            var serializedMessage = MessageFactory.GetSerializedMessage(message, rudp, out var messageId);
            if (ipAddressesStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
            {
                outgoingMessagesStorage.AddMessage(serializedMessage, ipEndPoint);
            }

            if (rudp)
            {
                rudpStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
            }
        }

        public void SendPlayerInfo(int matchId, int playerId, Dictionary<int, ushort> entityIds)
        {
            var message = new PlayerInfoMessage(entityIds);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendHides(int matchId, int playerId, ushort[] hiddenIds)
        {
            var message = new HidesMessage(hiddenIds);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendPositions(int matchId, int playerId, Dictionary<ushort, ViewTransform> entitiesInfo, bool rudp = false)
        {
            var message = new PositionsMessage { EntitiesInfo = entitiesInfo };
            SendUdp(matchId, playerId, message, rudp);
        }

        public void SendRadiuses(int matchId, int playerId, Dictionary<ushort, ushort> radiuses, bool rudp = false)
        {
            var message = new RadiusesMessage(radiuses);
            SendUdp(matchId, playerId, message, rudp);
        }

        public void SendParents(int matchId, int playerId, Dictionary<ushort, ushort> parents)
        {
            var message = new ParentsMessage(parents);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendDetaches(int matchId, int playerId, ushort[] detachedIds)
        {
            var message = new DetachesMessage(detachedIds);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendDestroys(int matchId, int playerId, ushort[] destroyedIds)
        {
            var message = new DestroysMessage(destroyedIds);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendKill(int matchId, KillData killData)
        {
            var playerId = killData.TargetPlayerId;
            var message = new KillMessage(killData.KillerId, killData.KillerType, killData.VictimId, killData.VictimType);
            SendUdp(matchId, playerId, message, true);
        }
        
        public void SendHealthPoints(int matchId, int targetPlayerId, float healthPoints)
        {
            var playerId = targetPlayerId;
            var message = new HealthPointsMessage(healthPoints);
            SendUdp(matchId, playerId, message);
        }
        
        public void SendMaxHealthPoints(int matchId, int playerId, float maxHealthPoints)
        {
            var message = new MaxHealthPointsMessage(maxHealthPoints);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendCooldown(int matchId, int targetPlayerId, float ability, float[] weapons)
        {
            var playerId = targetPlayerId;
            var message = new CooldownsMessage(ability, weapons);
            SendUdp(matchId, playerId, message);
        }

        public void SendCooldownInfo(int matchId, int playerId, float abilityCooldown, WeaponInfo[] weaponsInfos)
        {
            var message = new CooldownsInfosMessage(abilityCooldown, weaponsInfos);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendShieldPoints(int matchId, int playerId, float shieldPoints)
        {
            var message = new ShieldPointsMessage(shieldPoints);
            SendUdp(matchId, playerId, message);
        }

        public void SendMaxShieldPoints(int matchId, int playerId, float maxShieldPoints)
        {
            var message = new MaxShieldPointsMessage(maxShieldPoints);
            SendUdp(matchId, playerId, message, true);
        }

        public void SendShowAchievementsMessage(int matchId, int playerId)
        {
            var message = new ShowPlayerAchievementsMessage(matchId);
            SendUdp(matchId, playerId, message, true);
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