using System.Collections;
using System.Collections.Generic;

namespace Server.GameEngine
{
    public class PlayersViewAreas : IEnumerable<KeyValuePair<int, PlayersViewAreas.PlayerViewAreaInfo>>
    {
        public const float VisibleAreaRadius = 15f;
        private readonly Dictionary<int, PlayerViewAreaInfo> areas;
        public bool sendAll = false;

        public PlayersViewAreas(int playersCount)
        {
            areas = new Dictionary<int, PlayerViewAreaInfo>(playersCount);
        }
        
        public void Initialize(IEnumerable<int> playerIds)
        {
            foreach (var playerId in playerIds)
            {
                areas.Add(playerId, new PlayerViewAreaInfo());
            }
        }

        public IEnumerator<KeyValuePair<int, PlayerViewAreaInfo>> GetEnumerator()
        {
            return areas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public PlayerViewAreaInfo this[int playerId] => areas[playerId];

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