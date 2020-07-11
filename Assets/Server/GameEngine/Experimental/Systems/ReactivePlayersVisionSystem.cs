using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public abstract class ReactivePlayersVisionSystem : ReactiveSystem<GameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly PlayersViewAreas viewAreas;

        protected ReactivePlayersVisionSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas) : base(contexts.game)
        {
            this.matchId = matchId;
            viewAreas = playersViewAreas;
            this.udpSendUtils = udpSendUtils;
        }

        protected abstract void SendData(UdpSendUtils udpSendUtils, int matchId, ushort playerId, IEnumerable<GameEntity> entities);

        protected sealed override void Execute(List<GameEntity> entities)
        {
            foreach (var pair in viewAreas)
            {
                var playerVisibleObjects = GetVisibleObjects(pair.Value, entities);
                if (playerVisibleObjects.Count == 0) continue;
                SendData(udpSendUtils, matchId, pair.Key, playerVisibleObjects);
            }
        }

        private List<GameEntity> GetVisibleObjects(PlayersViewAreas.PlayerViewAreaInfo viewArea, IEnumerable<GameEntity> entities)
        {
            if (viewAreas.sendAll) return entities.ToList();

            var playerVisible = viewArea.lastVisible;

            return entities.Where(withView => playerVisible.Contains(withView.id.value)).ToList();
        }
    }
}