using System;
using System.Net;
using AmoebaBattleServer01.Experimental.GameEngine;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping;
using ZeroFormatter;

namespace AmoebaBattleServer01.Experimental.Udp.PlayerMessageHandlers
{
    public class PlayerPingHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            PlayerPingMessage mes = ZeroFormatterSerializer.Deserialize<PlayerPingMessage>(message.SerializedMessage);

            int gameRoomNumber = mes.GameRoomNumber;
            int playerId = mes.TemporaryId;

            TrySetUpIpAddress(sender, playerId);
            UpdateOrAddPingRecord(playerId);
        }

        private static void TrySetUpIpAddress(IPEndPoint sender, int playerId)
        {
            if (!BentMediator.PlayersIpAddressesWrapper.IsIpAddressAlreadyExists(sender))
            {
                BentMediator.PlayersIpAddressesWrapper.AddPlayer(playerId, sender);
                Console.WriteLine($"Ip нового игрока добавлен {sender.Address} {sender.Port} {sender.AddressFamily}");
            }
            else
            {
                Console.WriteLine($"Такой Ip уже был {sender.Address} {sender.Port} {sender.AddressFamily}");
            }
        }
        
        private static void UpdateOrAddPingRecord(int playerId)
        {
            if (PingLogger.LastPingTime.ContainsKey(playerId))
            {
                PingLogger.LastPingTime[playerId] = DateTime.UtcNow;
                // Console.WriteLine($"Успешно обновлена пинг запись от игрока {playerGoogleId}");
            }
            else
            {
                PingLogger.LastPingTime.TryAdd(playerId, DateTime.UtcNow);
            }
        }
    }
}