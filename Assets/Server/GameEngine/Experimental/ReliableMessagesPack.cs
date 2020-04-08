using System.Collections.Generic;
using System.Net;

namespace Server.GameEngine.Experimental
{
    public struct ReliableMessagesPack
    {
        // public int PlayerId;
        // public int MatchId;
        public readonly IPEndPoint IpEndPoint;
        public readonly Dictionary<uint, byte[]>.ValueCollection reliableMessages;

        public ReliableMessagesPack(IPEndPoint ipEndPoint, Dictionary<uint, byte[]>.ValueCollection reliableMessages)
        {
            IpEndPoint = ipEndPoint;
            this.reliableMessages = reliableMessages;
        }
    }
}