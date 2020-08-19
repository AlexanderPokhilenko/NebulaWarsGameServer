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
//         private readonly GameContext gameContext;
//         private readonly IGroup<GameEntity> playersGroup;
//         
//         public CooldownUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
//         {
//             this.matchId = matchId;
//             this.udpSendUtils = udpSendUtils;
//             gameContext = contexts.game;
//             playersGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
//         }
//
//         public void Execute()
//         {
//             foreach (var gameEntity in playersGroup)
//             {
//                 var playerId = gameEntity.player.id;
//
//                 var abilityCooldown = gameEntity.hasAbilityCooldown ? gameEntity.abilityCooldown.value : 0f;
//                 
//                 var weaponCooldowns = gameEntity.GetAllChildrenGameEntities(gameContext, c => c.hasCannon)
//                     .Select(e => e.hasCannonCooldown ? e.cannonCooldown.value : 0f).ToArray();
//
//                 udpSendUtils.SendCooldown(matchId, playerId, abilityCooldown, weaponCooldowns);
//             }    
//         }
//     }
// }