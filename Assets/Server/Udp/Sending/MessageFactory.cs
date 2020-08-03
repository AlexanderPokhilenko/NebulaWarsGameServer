using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace Server.Udp.Sending
{
    public class MessageFactory
    {
        private readonly MessageIdFactory messageIdFactory;

        public MessageFactory(MessageIdFactory messageIdFactory)
        {
            this.messageIdFactory = messageIdFactory;
        }
        
        public byte[] GetSerializedMessage<T>(T message, bool rudpEnabled, int matchId,  ushort playerId, out uint messageId)
            where T : ITypedMessage
        {
            MessageWrapper messageWrapper = GetMessage(message, rudpEnabled,matchId, playerId,  out messageId);
            return ZeroFormatterSerializer.Serialize(messageWrapper);
        }

        public byte[] GetSerializedMessage(MessageWrapper messageWrapper)
        {
            return ZeroFormatterSerializer.Serialize(messageWrapper);
        }
        
        private MessageWrapper GetMessage<T>(T mes, bool rudpEnabled,int matchId, ushort playerId, out uint messageId) 
            where T : ITypedMessage
        {
            byte[] serializedMessage = ZeroFormatterSerializer.Serialize(mes);
            MessageType messageType = mes.GetMessageType();
            messageId = messageIdFactory.CreateMessageId(matchId, playerId);
            MessageWrapper message = new MessageWrapper(messageType, serializedMessage, messageId, rudpEnabled);
            return message;
        }
    }
}