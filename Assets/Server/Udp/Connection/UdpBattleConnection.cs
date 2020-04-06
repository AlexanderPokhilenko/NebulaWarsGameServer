using System;
using System.Net;

namespace Server.Udp.Connection
{
    public class UdpBattleConnection:UdpConnection
    {
        private NetworkMediator mediator;
        
        public void SetMediator(NetworkMediator mediatorArg)
        {
            if (mediator != null)
            {
                throw new Exception("Повторная инициализация медиатора.");
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