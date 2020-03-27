﻿﻿﻿﻿﻿using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Http
{
    [ZeroFormattable]
    public class GameUnit
    {
        [Index(0)] public virtual int TemporaryId { get; set; }
        [Index(1)] public virtual bool IsBot { get; set; }
        [Index(2)] public virtual string PrefabName { get; set; }
        /// <summary>
        /// Показатель силы корабля
        /// </summary>
        [Index(3)] public virtual int WarshipCombatPowerLevel { get; set; }
    }
}