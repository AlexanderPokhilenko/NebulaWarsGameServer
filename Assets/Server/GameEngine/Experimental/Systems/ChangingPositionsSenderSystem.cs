using System.Collections.Generic;
using System.Linq;
using Entitas;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class ChangingPositionsSenderSystem : ReactivePlayersVisionSystem
    {
        public ChangingPositionsSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas) : base(contexts, matchId, udpSendUtils, playersViewAreas)
        { }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.ViewType));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasPosition && entity.hasDirection && entity.hasViewType;
        }

        protected override void SendData(UdpSendUtils udpSendUtils, int matchId, ushort playerId, IEnumerable<GameEntity> entities)
        {
            var visibleDict = entities.ToDictionary(e => e.id.value,
                e => new ViewTransform(e.position.value,
                    e.direction.angle,
                    e.viewType.id));

            udpSendUtils.SendPositions(matchId, playerId, visibleDict);
        }
    }
}