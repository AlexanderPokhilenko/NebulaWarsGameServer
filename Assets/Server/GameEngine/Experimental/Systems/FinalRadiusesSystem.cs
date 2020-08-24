// using System.Collections.Generic;
// using System.Linq;
// using Entitas;
// using Server.Udp.Sending;
// using UnityEngine;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     public class FinalRadiusesSystem : ReactivePlayersVisionSystem
//     {
//         public FinalRadiusesSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas) : base(contexts, matchId, udpSendUtils, playersViewAreas)
//         { }
//
//         protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
//         {
//             return context.CreateCollector(ServerGameMatcher.CircleScaling.Removed(), ServerGameMatcher.NonstandardRadius.Added());
//         }
//
//         protected override bool Filter(ServerGameEntity entity)
//         {
//             return entity.hasCircleCollider && !entity.hasCircleScaling;
//         }
//
//         protected override void SendData(UdpSendUtils udpSendUtils, int matchId, ushort playerId, IEnumerable<ServerGameEntity> entities)
//         {
//             var dict = entities.ToDictionary(e => e.id.value, e => Mathf.FloatToHalf(e.circleCollider.radius));
//             udpSendUtils.SendRadiuses(matchId, playerId, dict, true);
//         }
//     }
// }