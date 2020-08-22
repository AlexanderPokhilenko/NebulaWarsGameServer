using System.Net;

namespace Server.Udp.Connection
{
    public interface IByteArrayHandler
    {
        void HandleBytes(byte[] data, IPEndPoint endPoint);
    }
}