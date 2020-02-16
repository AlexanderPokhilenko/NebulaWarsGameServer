using System.Net;

namespace OldServer.Experimental.Udp.Connection
{
    public class UdpBattleConnection:UdpConnection
    {
        private readonly NetworkMediator mediator;
        
        public UdpBattleConnection(NetworkMediator mediator)
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