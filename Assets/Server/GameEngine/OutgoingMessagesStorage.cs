using System.Collections.Generic;
using System.Net;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    /// <summary>
    /// Хранит сообщеения для всех игроков на протяжении кадра.
    /// По его окончании все сообщения отправляются игрокам и хранилище очищается для следующего кадра.
    /// </summary>
    public class OutgoingMessagesStorage
    {
        private readonly SimpleDatagramPacker simpleDatagramPacker;
        private readonly Dictionary<IPEndPoint, List<byte[]>> messages = new Dictionary<IPEndPoint, List<byte[]>>();
        
        public OutgoingMessagesStorage(SimpleDatagramPacker simpleDatagramPacker)
        {
            this.simpleDatagramPacker = simpleDatagramPacker;
        }
        
        public void AddMessage(byte[] data, IPEndPoint ipEndPoint)
        {
            if (messages.TryGetValue(ipEndPoint, out List<byte[]> playerMessages))
            {
                playerMessages.Add(data);
            }
            else
            {
                //TODO сколько сообщений отправляется игроку за кадр в среднем?
                messages.Add(ipEndPoint, new List<byte[]>(5){data} );
            }
        }
        
        public void SendAllMessages()
        {
            foreach (KeyValuePair<IPEndPoint, List<byte[]>> pair in messages)
            {
                simpleDatagramPacker.Send(pair.Key, pair.Value);
            }
            
            messages.Clear();
        }
    }
}