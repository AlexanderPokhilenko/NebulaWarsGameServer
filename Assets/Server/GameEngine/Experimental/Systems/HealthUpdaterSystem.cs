using Entitas;
using Server.Udp.Sending;
using System.Collections.Generic;
using System.Linq;

namespace Server.GameEngine.Experimental.Systems
{
    //TODO: система не учитывает, что пакеты могут быть потеряны
    public class HealthUpdaterSystem : ReactivePlayersVisionSystem
    {
        public HealthUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas) : base(contexts, matchId, udpSendUtils, playersViewAreas)
        { }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.HealthPoints);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasHealthPoints && entity.hasViewType && !entity.isDestroyed;
        }

        protected override void SendData(UdpSendUtils udpSendUtils, int matchId, ushort playerId, IEnumerable<GameEntity> entities)
        {
            var dict = entities.ToDictionary(e => e.id.value, e =>
            {
                var hp = e.healthPoints.value;
                if (hp > 0f)
                {
                    return (ushort) hp;
                }
                return (ushort)0;
            });
            udpSendUtils.SendHealthPoints(matchId, playerId, dict);
        }
    }
}