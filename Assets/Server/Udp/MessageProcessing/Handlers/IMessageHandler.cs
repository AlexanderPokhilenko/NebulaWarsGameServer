using System.Net;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;

namespace Server.Udp.MessageProcessing.Handlers
{
    public interface IMessageHandler
    {
        void Handle(MessageWrapper messageWrapper, IPEndPoint sender);
    }
}