using System.Net;

namespace Server.Udp.Sending
{
    public interface IUdpSender
    {
        void Send(byte[] data, IPEndPoint endPoint);
    }
}