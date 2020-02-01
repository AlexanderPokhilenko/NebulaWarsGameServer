using System.Net;

namespace AmoebaBattleServer01.Experimental.Udp
{
    public class UdpBattleConnection:UdpConnection
    {
        private readonly BentMediator mediator;
        
        public UdpBattleConnection(BentMediator mediator)
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