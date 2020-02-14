using System.Net;
using AmoebaBattleServer01.Experimental;

namespace OldServer.Experimental.Udp
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