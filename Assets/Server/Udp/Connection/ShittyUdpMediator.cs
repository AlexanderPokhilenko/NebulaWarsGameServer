using System;
using System.Net;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using Server.Udp.MessageProcessing;
using ZeroFormatter;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Принимает все udp сообщения от игроков.
    /// </summary>
    public class ShittyUdpMediator:UdpClientWrapper
    {
        private MessageProcessor messageProcessor;

        public void SetProcessor(MessageProcessor messageProcessorArg)
        {
            if (messageProcessor == null)
            {
                messageProcessor = messageProcessorArg;
            }
            else
            {
                throw new Exception("Обработчик пакетов уже был установлен.");
            }
        }
        
        protected override void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            messageProcessor.Handle(messageWrapper, endPoint);
        }
    }
}