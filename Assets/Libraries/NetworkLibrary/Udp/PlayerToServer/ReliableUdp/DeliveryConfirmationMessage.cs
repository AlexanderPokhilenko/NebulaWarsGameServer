using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace Libraries.NetworkLibrary.Udp.PlayerToServer.ReliableUdp
{
    public class DeliveryConfirmationMessage:ITypedMessage
    {
        [Index(0)] public int PlayerId { get; set; }
        [Index(1)] public uint MessageNumberThatConfirms { get; set; }
        
        public MessageType GetMessageType() => MessageType.DeliveryConfirmationFromClient;
    }
}