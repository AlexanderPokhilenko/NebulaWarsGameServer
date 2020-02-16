using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace Libraries.NetworkLibrary.Udp.ServerToPlayer
{
    public struct DeliveryConfirmationFromServerMessage:ITypedMessage
    {
        [Index(0)] public uint MessageNumberThatConfirms { get; set; }
        public MessageType GetMessageType() => MessageType.DeliveryConfirmationFromClient;
    }
}