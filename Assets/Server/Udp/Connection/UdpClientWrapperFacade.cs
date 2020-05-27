using System;
using System.Net;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Принимает все udp сообщения от игроков.
    /// </summary>
    public class UdpClientWrapperFacade:UdpClientWrapper
    {
        private UdpMediator mediator;

        public void SetMediator(UdpMediator mediatorArg)
        {
            if (mediator == null)
            {
                mediator = mediatorArg;
            }
            else
            {
                throw new Exception("Мудиатор уже был установлен");
            }
        }
        
        protected override void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            base.HandleBytes(data, endPoint);
            mediator.HandleBytes(data, endPoint);
        }
    }
}