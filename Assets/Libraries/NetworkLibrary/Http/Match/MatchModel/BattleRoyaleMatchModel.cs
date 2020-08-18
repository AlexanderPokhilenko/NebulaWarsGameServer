﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections.Generic;
                  using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Http
{
    /// <summary>
    /// Нужен для передачи данных о бое между матчмейкером и игровым сервером.
    /// </summary>
    [ZeroFormattable]
    public class BattleRoyaleMatchModel
    {
        [Index(0)] public virtual string GameServerIp{ get; set; }
        [Index(1)] public virtual int GameServerPort{ get; set; }
        [Index(2)] public virtual int MatchId{ get; set; }
        [Index(3)] public virtual GameUnits GameUnits { get; set; }

        public List<ushort> GetPlayerTmpIds()
        {
            List<ushort> result = new List<ushort>(GameUnits.Count());
            foreach (var playerModel in GameUnits.Players)
            {
                result.Add(playerModel.TemporaryId);
            }
            
            foreach (var botModel in GameUnits.Bots)
            {
                result.Add(botModel.TemporaryId);
            }

            return result;
        }
    }
}