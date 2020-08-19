using System;
using System.Collections.Generic;
using Plugins.submodules.SharedCode.Logger;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    /// <summary>
    /// Хранит сообщеения для всех игроков на протяжении кадра.
    /// По его окончании все сообщения отправляются игрокам и хранилище очищается для следующего кадра.
    /// </summary>
    public class OutgoingMessagesStorage
    {
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly SimpleMessagesPacker simpleMessagesPacker;
        private readonly ILog log = LogManager.CreateLogger(typeof(OutgoingMessagesStorage));
        private readonly Dictionary<Tuple<int, ushort>, List<byte[]>> messages = new Dictionary<Tuple<int, ushort>, List<byte[]>>();
        
        public OutgoingMessagesStorage(SimpleMessagesPacker simpleMessagesPacker, IpAddressesStorage ipAddressesStorage)
        {
            this.simpleMessagesPacker = simpleMessagesPacker;
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public void AddMessage(int matchId, ushort playerId, byte[] data)
        {
            if (!ipAddressesStorage.Contains(matchId, playerId))
            {
                return;
            }


            Tuple<int, ushort> id = new Tuple<int, ushort>(matchId, playerId);
            if (messages.TryGetValue(id, out List<byte[]> playerMessages))
            {
                playerMessages.Add(data);
            }
            else
            {
                messages.Add(id, new List<byte[]>{data} );
            }
        }
        
        public void SendAllMessages()
        {
            foreach (KeyValuePair<Tuple<int, ushort>, List<byte[]>> pair in messages)
            {
                int matchId = pair.Key.Item1;
                ushort playerId = pair.Key.Item2;
                List<byte[]> serializedMessages = pair.Value;
                if(!ipAddressesStorage.TryGetIpEndPoint(matchId, playerId, out var ip))
                {
                    log.Debug($"Ip игрока {nameof(matchId)} {matchId} {nameof(playerId)} {playerId} нет.");
                    return;
                }
                simpleMessagesPacker.Send(matchId, playerId, ip, serializedMessages);
            }
            
            messages.Clear();
        }
    }
}