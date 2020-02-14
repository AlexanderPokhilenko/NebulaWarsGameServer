using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp
{
    [ZeroFormattable]
    public struct Message
    {
        [Index(0)] public MessageType MessageType;
        [Index(1)] public byte[] SerializedMessage;
        [Index(2)] public uint MessageId;
        [Index(3)] public bool NeedResponse;
        
        public Message(MessageType messageType, byte[] serializedMessage, uint messageId, bool needResponse)
        {
            MessageType = messageType;
            SerializedMessage = serializedMessage;
            MessageId = messageId;
            NeedResponse = needResponse;
        }
    }

    public interface ITypedMessage
    {
        MessageType GetMessageType();
    }

    public enum MessageType
    {
        PlayerInput = 3,
        PlayerPing = 5,
        Positions = 6,
        DeliveryConfirmation = 7
    }
}