﻿﻿using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace Libraries.NetworkLibrary.Udp.ServerToPlayer
{
    [ZeroFormattable]
    public struct BattleFinishMessage:ITypedMessage
    {
        public MessageType GetMessageType()
        {
            return MessageType.BattleFinish;
        }
    }
}