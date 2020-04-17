using System.Net;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Принимает все udp сообщения от игроков.
    /// </summary>
    public class UdpListenerFacade:UdpListener
    {
        private readonly UdpMediator mediator;

        public UdpListenerFacade(UdpMediator mediator)
        {
            this.mediator = mediator;
        }

        protected override void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            base.HandleBytes(data, endPoint);
            mediator.HandleBytes(data, endPoint);
        }
    }
}