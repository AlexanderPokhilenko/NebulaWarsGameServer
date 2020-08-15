using System.Collections.Generic;

namespace Server.Udp.Sending
{
    public interface IHealthPointsPackSender
    {
        void SendHealthPointsPack(int matchId, ushort tmpPlayerId, Dictionary<ushort, float> entityIdToValue);
    }
    
    public interface IMaxHealthPointsPackSender
    {
        void SendMaxHealthPointsPack(int matchId, ushort tmpPlayerId, Dictionary<ushort, float> entityIdToValue);
    }
}