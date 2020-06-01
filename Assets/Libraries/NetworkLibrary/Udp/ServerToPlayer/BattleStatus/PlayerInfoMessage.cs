using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus
{
    [ZeroFormattable]
    public struct PlayerInfoMessage : ITypedMessage
    {
        [Index(0)] public readonly ushort EntityId;

        public PlayerInfoMessage(ushort entityId)
        {
            EntityId = entityId;
        }

        public MessageType GetMessageType()
        {
            return MessageType.PlayerInfo;
        }
    }
}