using ZeroFormatter;

// ReSharper disable once CheckNamespace
namespace NetworkLibrary.NetworkLibrary.Udp
{
    [ZeroFormattable]
    public struct MessagesContainer
    {
        [Index(0)] public byte[][] MessageWrappers;
        
        public MessagesContainer(byte[][] messageWrappers)
        {
            MessageWrappers = messageWrappers;
        }

        public bool CanAccommodateANewItem(int itemLength)
        {
            return MessageWrappers.Length + itemLength < MessageContainerMaxSizeInBytes;
        }
        
        public const int MessageContainerMaxSizeInBytes = 1500;
    }
}