using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp
{
    public static class MessageFactory
    {
        public static Message GetMessage<T>(T mes, bool needResponse = false) where T : ITypedMessage
        {
            var serializedMessage = ZeroFormatterSerializer.Serialize(mes);
            var messageType = mes.GetMessageType();
            var messageId = MessageIdGenerator.GetMessageId();
            var message = new Message(messageType, serializedMessage, messageId, needResponse);
            return message;
        }

        public static byte[] GetSerializedMessage<T>(T message, bool needResponse = false) where T : ITypedMessage
        {
            return ZeroFormatterSerializer.Serialize(GetMessage(message, needResponse));
        }

        public static byte[] GetSerializedMessage(Message message)
        {
            return ZeroFormatterSerializer.Serialize(message);
        }
    }

    public static class MessageIdGenerator
    {
        private static uint lastMessageId;
        public static uint GetMessageId()
        {
            return lastMessageId++;
        }
    }
}