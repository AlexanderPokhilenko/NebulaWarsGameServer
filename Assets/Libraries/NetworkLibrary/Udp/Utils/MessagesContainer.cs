﻿﻿﻿﻿﻿using System.Collections.Generic;
     using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp
{
    [ZeroFormattable]
    public class MessagesContainer
    {
        [Index(0)] public virtual byte[][] Messages { get; set; }
    }
}