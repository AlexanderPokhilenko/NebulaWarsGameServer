﻿using System;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping;
using Server.GameEngine.Experimental;
using Server.Udp.Connection;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PingMessageHandler:IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UdpConnection));
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            PlayerPingMessage mes = 
                ZeroFormatterSerializer.Deserialize<PlayerPingMessage>(messageWrapper.SerializedMessage);
            
            int playerId = mes.TemporaryId;

            TrySetUpIpAddress(sender, playerId);
            UpdateOrAddPingRecord(playerId);
        }

        private static void TrySetUpIpAddress(IPEndPoint sender, int playerId)
        {
            if (!NetworkMediator.IpAddressesStorage.IsIpAddressAlreadyExists(sender))
            {
                NetworkMediator.IpAddressesStorage.AddPlayer(playerId, sender);
                Log.Info($"Ip нового игрока добавлен {sender.Address} {sender.Port} {sender.AddressFamily}");
            }
            else
            {
                // Log.Info($"Такой Ip уже был {sender.Address} {sender.Port} {sender.AddressFamily}");
            }
        }
        
        private static void UpdateOrAddPingRecord(int playerId)
        {
            if (PingLogger.LastPingTime.ContainsKey(playerId))
            {
                PingLogger.LastPingTime[playerId] = DateTime.UtcNow;
                // Log.Info($"Успешно обновлена пинг запись от игрока {playerGoogleId}");
            }
            else
            {
                PingLogger.LastPingTime.TryAdd(playerId, DateTime.UtcNow);
            }
        }
    }
}