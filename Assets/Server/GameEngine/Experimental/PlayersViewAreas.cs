using System.Collections.Generic;

namespace Server.GameEngine
{
    public class PlayersViewAreas
    {
        public const float VisibleAreaRadius = 15f;
        private readonly Dictionary<int, PlayerViewAreaInfo> areas;

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

        public PlayerViewAreaInfo this[int playerId] => areas[playerId];

        public class PlayerViewAreaInfo
        {
            public readonly HashSet<ushort> lastVisible;

            public PlayerViewAreaInfo()
            {
                lastVisible = new HashSet<ushort>();
            }
        }
    }
}