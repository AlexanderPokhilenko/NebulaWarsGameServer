using System.Collections.Generic;
using System.Linq;
using Plugins.submodules.SharedCode.LagCompensation;

namespace Server.GameEngine
{
    public class ServerSnapshotHistory : IServerSnapshotHistory
    {
        private readonly Dictionary<int, SnapshotWithTime> history =
            new Dictionary<int, SnapshotWithTime>();
        
        public SnapshotWithTime Get(int tickNumber)
        {
            return history[tickNumber];
        }

        public int GetLastTickNumber()
        {
            if (history.Keys.Count == 0)
            {
                return -1;
            }
            
            return history.Keys.Max();
        }

        public void Add(SnapshotWithTime snapshotWithTime)
        {
            history.Add(snapshotWithTime.tickNumber, snapshotWithTime);
        }

        public SnapshotWithTime GetActualGameState()
        {
            return history[GetLastTickNumber()];
        }

        public float GetLastTickTime()
        {
            return history[GetLastTickNumber()].tickTime;
        }
    }
}