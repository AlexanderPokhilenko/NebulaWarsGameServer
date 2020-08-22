using System;
using System.Net;
using Plugins.submodules.SharedCode;
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
        private MessageWrapperHandler messageWrapperHandler;

        public void SetMessageWrapperHandler(MessageWrapperHandler messageWrapperHandlerArg)
        {
            if (messageWrapperHandler == null)
            {
                messageWrapperHandler = messageWrapperHandlerArg;
            }
            else
            {
                throw new Exception("Обработчик пакетов уже был установлен.");
            }
        }
        
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            messageWrapperHandler.Handle(messageWrapper, endPoint);
        }
    }
}