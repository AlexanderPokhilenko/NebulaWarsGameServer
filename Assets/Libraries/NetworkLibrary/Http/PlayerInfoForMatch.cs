﻿﻿﻿﻿using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Http
{
    /// <summary>
    /// Нужен для гейм сервера. Хранит всю информацию, которая влияет на бой.
    /// </summary>
    [ZeroFormattable]
    public class PlayerInfoForMatch:GameUnit
    {
        /// <summary>
        /// Username игрока
        /// </summary>
        [Index(4)] public virtual string ServiceId { get; set; }
        //TODO какое-то говнище
        [Index(5)] public virtual int AccountId { get; set; }
        [Index(6)] public virtual string SomeDichMegaData { get; set; }
    }
}