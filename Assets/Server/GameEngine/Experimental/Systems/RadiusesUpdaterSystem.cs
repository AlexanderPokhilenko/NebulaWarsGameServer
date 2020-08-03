using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;
using UnityEngine;

namespace Server.GameEngine.Experimental.Systems
{
    public class RadiusesUpdaterSystem : ReactivePlayersVisionSystem
    {
        public RadiusesUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas) : base(contexts, matchId, udpSendUtils, playersViewAreas)
        { }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.CircleCollider);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCircleCollider && entity.hasCircleScaling;
        }

        protected override void SendData(UdpSendUtils udpSendUtils, int matchId, ushort playerId, IEnumerable<GameEntity> entities)
        {
            var dict = entities.ToDictionary(e => e.id.value, e => Mathf.FloatToHalf(e.circleCollider.radius));
            udpSendUtils.SendRadiuses(matchId, playerId, dict);
        }
    }
}