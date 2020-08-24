// using System;
// using System.Linq;
// using Entitas;
// using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
// using Server.Udp.Sending;
// using UnityEngine;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     public class UnhiddenStoppedSenderSystem : IExecuteSystem
//     {
//         private readonly int matchId;
//         private readonly UdpSendUtils udpSendUtils;
//         private readonly PlayersViewAreas viewAreas;
//         private readonly ServerGameContext gameContext;
//         private readonly IGroup<ServerGameEntity> withTransforms;
//         private readonly IGroup<ServerGameEntity> withRadiuses;
//
//         public UnhiddenStoppedSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas)
//         {
//             this.matchId = matchId;
//             viewAreas = playersViewAreas;
//             this.udpSendUtils = udpSendUtils;
//             gameContext = contexts.serverGame;
//             withTransforms = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.ViewType,
//                 ServerGameMatcher.Position,
//                 ServerGameMatcher.Direction).NoneOf(ServerGameMatcher.Moving));
//             withRadiuses = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.ViewType,
//                     ServerGameMatcher.CircleCollider,
//                     ServerGameMatcher.NonstandardRadius)
//                 .NoneOf(ServerGameMatcher.CircleScaling));
//         }
//
//         public void Execute()
//         {
//             throw new NotImplementedException();
//             // foreach (var pair in viewAreas)
//             // {
//             //     var playerId = pair.Key;
//             //     var unhidden = pair.Value.newUnhidden;
//             //
//             //     var unhiddenTransforms = withTransforms.AsEnumerable().Where(e => unhidden.Contains(e.id.value)).ToList();
//             //     if (unhiddenTransforms.Count > 0)
//             //     {
//             //         var positions = unhiddenTransforms.ToDictionary(e => e.id.value,
//             //             e => new ViewTransform(e.position.value,
//             //                 e.direction.angle,
//             //                 e.viewType.id));
//             //
//             //         udpSendUtils.SendPositions(matchId, playerId, positions, true);
//             //     }
//             //
//             //     var unhiddenRadiuses = withRadiuses.AsEnumerable().Where(e => unhidden.Contains(e.id.value)).ToList();
//             //     if (unhiddenRadiuses.Count > 0)
//             //     {
//             //         var radiuses = unhiddenRadiuses.ToDictionary(e => e.id.value, e => Mathf.FloatToHalf(e.circleCollider.radius));
//             //         udpSendUtils.SendRadiuses(matchId, playerId, radiuses, true);
//             //     }
//             // }
//         }
//     }
// }