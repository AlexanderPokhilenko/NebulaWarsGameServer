using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;

namespace Server.Udp.MessageProcessing.Handlers
{
    public interface IMessageHandler
    {
        void Handle(MessageWrapper messageWrapper, IPEndPoint sender);
    }
}