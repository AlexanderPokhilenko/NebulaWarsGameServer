using System.Net;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using Server.Udp.MessageProcessing;
using ZeroFormatter;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Принимает все udp сообщения от игроков.
    /// </summary>
    public class ByteArrayHandler:IByteArrayHandler
    {
        private readonly MessageWrapperHandler messageWrapperHandler;

        public ByteArrayHandler(MessageWrapperHandler messageWrapperHandler)
        {
            this.messageWrapperHandler = messageWrapperHandler;
        }
        
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            messageWrapperHandler.Handle(messageWrapper, endPoint);
        }
    }
}