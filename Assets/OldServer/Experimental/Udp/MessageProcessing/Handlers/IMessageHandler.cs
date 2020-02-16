using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;

namespace OldServer.Experimental.Udp.MessageProcessing.Handlers
{
    public interface IMessageHandler
    {
        void Handle(Message message, IPEndPoint sender);
    }
}