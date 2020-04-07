using System;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping;
using Server.GameEngine;
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
            int matchId = mes.GameRoomNumber;

            TrySetUpIpAddress(sender,matchId, playerId);
            UpdateOrAddPingRecord(playerId);
        }

        private static void TrySetUpIpAddress(IPEndPoint ipEndPoint, int matchId, int playerId)
        {
            if (!GameEngineMediator.MatchStorageFacade.ContainsIpEndPoint(matchId, playerId))
            {
                GameEngineMediator.MatchStorageFacade.AddEndPoint(matchId, playerId, ipEndPoint);
                Log.Info($"Ip нового игрока добавлен {ipEndPoint.Address} {ipEndPoint.Port} {ipEndPoint.AddressFamily}");
            }
            else
            {
                // Log.Info($"Такой Ip уже был {ipEndPoint.Address} {ipEndPoint.Port} {ipEndPoint.AddressFamily}");
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