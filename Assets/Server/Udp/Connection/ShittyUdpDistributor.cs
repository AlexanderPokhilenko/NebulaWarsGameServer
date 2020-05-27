using System;
using System.Net;
using System.Threading;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.MessageProcessing;
using ZeroFormatter;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Принимает все udp сообщения от игроков.
    /// </summary>
    /// //TODO говно
    public class ShittyUdpDistributor:UdpClientWrapper
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
                throw new Exception("Обрабтчик пакетов уже был установлен.");
            }
        }
        
        protected override void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            messageProcessor.Handle(messageWrapper, endPoint);
        }
    }
}