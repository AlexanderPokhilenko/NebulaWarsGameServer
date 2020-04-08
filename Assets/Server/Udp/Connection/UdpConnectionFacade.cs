using System;
using System.Net;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Принимает все udp сообщения от игроков.
    /// </summary>
    public class UdpConnectionFacade:UdpConnection
    {
        private NetworkMediator mediator;
        
        public void SetMediator(NetworkMediator mediatorArg)
        {
            if (mediator != null)
            {
                throw new Exception("Повторная инициализация.");
            }
            else
            {
                mediator = mediatorArg;
            }
        }
        
        protected override void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            base.HandleBytes(data, endPoint);
            mediator.HandleBytes(data, endPoint);
        }
    }
}