using System;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    public class RudpMessagesSender
    {
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;

        public RudpMessagesSender(ByteArrayRudpStorage byteArrayRudpStorage)
        {
            this.byteArrayRudpStorage = byteArrayRudpStorage;
        }
        
        public void SendAll()
        {
            throw new NotImplementedException();
        }
    }
}