using System.Collections;
using System.Collections.Generic;

namespace Server.GameEngine.Experimental
{
    public class PlayersViewAreas : IEnumerable<KeyValuePair<ushort, PlayersViewAreas.PlayerViewAreaInfo>>
    {
        public const float VisibleAreaRadius = 15f;
        private readonly Dictionary<ushort, PlayerViewAreaInfo> areas;
        public readonly int Count;
        public bool sendAll = false;

        public PlayersViewAreas(int playersCount)
        {
            Count = playersCount;
            areas = new Dictionary<ushort, PlayerViewAreaInfo>(playersCount);
        }
        
        public void Initialize(IEnumerable<ushort> playerIds)
        {
            foreach (var playerId in playerIds)
            {
                areas.Add(playerId, new PlayerViewAreaInfo());
            }
        }

        public IEnumerator<KeyValuePair<ushort, PlayerViewAreaInfo>> GetEnumerator()
        {
            return areas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public PlayerViewAreaInfo this[ushort playerId] => areas[playerId];

        public class PlayerViewAreaInfo
        {
            public readonly HashSet<ushort> lastVisible;
            public readonly HashSet<ushort> newUnhidden;

            public PlayerViewAreaInfo()
            {
                lastVisible = new HashSet<ushort>();
                newUnhidden = new HashSet<ushort>();
            }
        }
    }
}