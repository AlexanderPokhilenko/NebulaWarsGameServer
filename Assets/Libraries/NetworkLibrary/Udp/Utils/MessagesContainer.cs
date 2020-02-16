﻿﻿﻿using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp
{
    [ZeroFormattable]
    public struct MessagesContainer
    {
        [Index(0)] public Message[] Messages;
        public MessagesContainer(Message[] messages)
        {
            Messages = messages;
        }
    }
}