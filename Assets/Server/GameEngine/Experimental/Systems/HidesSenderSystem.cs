// using System.Collections.Generic;
// using System.Linq;
// using Entitas;
// using Server.Udp.Sending;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     /// <summary>
//     /// Отвечает за радиус обзора и отправляет спрятанные обьекты.
//     /// </summary>
//     public class HidesSenderSystem : IExecuteSystem, IInitializeSystem
//     {
//         private readonly int matchId;
//         private readonly UdpSendUtils udpSendUtils;
//         private readonly IGroup<ServerGameEntity> players;
//         private readonly IGroup<ServerGameEntity> grandObjects;
//         private readonly IGroup<ServerGameEntity> removedObjects;
//         private ServerGameEntity zone;
//         private readonly ServerGameContext gameContext;
//         private readonly List<ushort> removedObjectIdsBuffer;
//         private readonly PlayersViewAreas viewAreas;
//         private const float visibleAreaRadius = PlayersViewAreas.VisibleAreaRadius;
//
//         public HidesSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas)
//         {
//             this.matchId = matchId;
//             this.udpSendUtils = udpSendUtils;
//             viewAreas = playersViewAreas;
//             gameContext = contexts.serverGame;
//             //todo добавить сюда observer
//             players = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
//             var grandMatcher = ServerGameMatcher.AllOf(ServerGameMatcher.GlobalTransform).NoneOf(ServerGameMatcher.Parent);
//             grandObjects = gameContext.GetGroup(grandMatcher);
//             removedObjects = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Destroyed, ServerGameMatcher.Position, ServerGameMatcher.Direction, ServerGameMatcher.ViewType));
//             removedObjectIdsBuffer = new List<ushort>();
//         }
//
//         public void Initialize()
//         {
//             zone = gameContext.zone.GetZone(gameContext);
//         }
//
//         public void Execute()
//         {
//             if(viewAreas.sendAll) return;
//             if (zone.circleCollider.radius <= visibleAreaRadius) viewAreas.sendAll = true;
//
//             removedObjectIdsBuffer.Clear();
//             removedObjectIdsBuffer.AddRange(removedObjects.AsEnumerable().Select(e => e.id.value));
//             foreach (var player in players)
//             {
//                 var playerId = player.player.id;
//                 var viewArea = viewAreas[playerId];
//                 var playerVisible = GetVisible(player);
//
//                 var last = viewArea.lastVisible;
//                 var unhidden = viewArea.newUnhidden;
//
//                 unhidden.Clear();
//                 unhidden.UnionWith(playerVisible);
//                 unhidden.ExceptWith(removedObjectIdsBuffer);
//                 unhidden.ExceptWith(last);
//
//                 last.ExceptWith(playerVisible);
//                 last.ExceptWith(removedObjectIdsBuffer);
//                 if (last.Count > 0) udpSendUtils.SendHides(matchId, playerId, last.ToArray());
//
//                 last.Clear();
//                 last.UnionWith(playerVisible);
//                 last.ExceptWith(removedObjectIdsBuffer);
//             }
//         }
//
//         private List<ushort> GetVisible(ServerGameEntity currentPlayer)
//         {
//             var result = new List<ushort>();
//             var currentPlayerPosition = currentPlayer.globalTransform.position;
//
//             foreach (var e in grandObjects)
//             {
//                 var radius = e.hasCircleCollider ? e.circleCollider.radius : 0;
//                 if ((e.globalTransform.position - currentPlayerPosition).magnitude - radius > visibleAreaRadius) continue;
//
//                 var viewChildren = e.GetAllChildrenGameEntities(gameContext, c => c.hasViewType);
//
//                 result.AddRange(viewChildren.Select(child => child.id.value));
//             }
//
//             return result;
//         }
//     }
// }