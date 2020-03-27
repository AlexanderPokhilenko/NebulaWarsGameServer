﻿using System;
 using System.Collections;
 using System.Collections.Generic;
using ZeroFormatter;

//TODO протестировать индексатор

namespace NetworkLibrary.NetworkLibrary.Http
{
    [ZeroFormattable]
    public class GameUnitsForMatch
    {
        [Index(0)] public virtual List<PlayerInfoForMatch> Players { get; set; }
        [Index(1)] public virtual List<BotInfo> Bots { get; set; }

        [IgnoreFormat] 
        public GameUnit this[int index]
        {
            get
            {
                if (index <= Players?.Count)
                {
                    return Players[index];
                }
                else if (index <= Bots.Count + Players?.Count)
                {
                    return Bots[index - Players.Count];
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
        
        public int Count()
        {
            int sum = 0;
            if (Players != null)
            {
                sum += Players.Count;
            }

            if (Bots != null)
            {
                sum += Bots.Count;
            }
            
            return sum;
        }
    }
}