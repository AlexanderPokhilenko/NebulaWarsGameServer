using System;
using System.Collections;
using System.Collections.Generic;

namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Разбивает игровое соостояие на наборы, которые нужно обработать в разных тиках
    /// </summary>
    public class TimeTravelMap
    {
        public class Bucket : IEnumerable
        {
            public int Time { get; set; }
            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        public List<Bucket> RefillBuckets(GameState gs)
        {
            throw new NotImplementedException();
        }
    }
}