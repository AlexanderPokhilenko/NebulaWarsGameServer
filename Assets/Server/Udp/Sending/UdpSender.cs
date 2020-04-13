using System.Net;
using System.Net.Sockets;
using log4net;

namespace Server.Udp.Sending
{
    public class UdpSender
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UdpSender));
        private readonly UdpClient udpClient;
        public UdpSender()
        {
            udpClient = new UdpClient();
        }

        public void Send(byte[] data, IPEndPoint endPoint)
        {
            if (data != null)
            {
                try
                { 
                    udpClient.Send(data, data.Length, endPoint);
                }
                catch (SocketException)
                {
                    //ignore   
                }    
            }
            else
            {
                Log.Warn("Отправляемые данные пусты");
            }
            
        }
    }
}