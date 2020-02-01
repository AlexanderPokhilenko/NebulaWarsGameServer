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
            string playerGoogleId = mes.PlayerGoogleId;

            TrySetUpIpAddress(sender, playerGoogleId);
            UpdateOrAddPingRecord(playerGoogleId);
        }

        private static void TrySetUpIpAddress(IPEndPoint sender, string playerGoogleId)
        {
            if (!BentMediator.PlayersIpAddressesWrapper.IsIpAddressAlreadyExists(sender))
            {
                BentMediator.PlayersIpAddressesWrapper.AddPlayer(playerGoogleId, sender);
                Console.WriteLine($"Ip нового игрока добавлен {sender.Address} {sender.Port} {sender.AddressFamily}");
            }
            else
            {
                Console.WriteLine($"Такой Ip уже был {sender.Address} {sender.Port} {sender.AddressFamily}");
            }
        }
        
        private static void UpdateOrAddPingRecord(string playerGoogleId)
        {
            if (PingLogger.LastPingTime.ContainsKey(playerGoogleId))
            {
                PingLogger.LastPingTime[playerGoogleId] = DateTime.UtcNow;
                // Console.WriteLine($"Успешно обновлена пинг запись от игрока {playerGoogleId}");
            }
            else
            {
                while (!PingLogger.LastPingTime.TryAdd(playerGoogleId, DateTime.UtcNow))
                {
                    
                }
            }
        }
    }
}