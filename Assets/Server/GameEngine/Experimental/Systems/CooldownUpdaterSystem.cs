// using System.Linq;
// using Entitas;
// using Server.Udp.Sending;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     //TODO отправлять сообщения только при изменении
//     public class CooldownUpdaterSystem : IExecuteSystem
//     {
//         private readonly int matchId;
//         private readonly UdpSendUtils udpSendUtils;
//         private readonly ServerGameContext gameContext;
//         private readonly IGroup<ServerGameEntity> playersGroup;
//         
//         public CooldownUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
//         {
//             this.matchId = matchId;
//             this.udpSendUtils = udpSendUtils;
//             gameContext = contexts.serverGame;
//             playersGroup = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
//         }
//
//         public void Execute()
//         {
//             foreach (var ServerGameEntity in playersGroup)
//             {
//                 var playerId = ServerGameEntity.player.id;
//
//                 var abilityCooldown = ServerGameEntity.hasAbilityCooldown ? ServerGameEntity.abilityCooldown.value : 0f;
//                 
//                 var weaponCooldowns = ServerGameEntity.GetAllChildrenGameEntities(gameContext, c => c.hasCannon)
//                     .Select(e => e.hasCannonCooldown ? e.cannonCooldown.value : 0f).ToArray();
//
//                 udpSendUtils.SendCooldown(matchId, playerId, abilityCooldown, weaponCooldowns);
//             }    
//         }
//     }
// }