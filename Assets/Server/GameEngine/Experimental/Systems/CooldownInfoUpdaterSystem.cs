// using System.Collections.Generic;
// using System.Linq;
// using Entitas;
// using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
// using Server.Udp.Sending;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     public class CooldownInfoUpdaterSystem : IExecuteSystem, IInitializeSystem
//     {
//         private readonly int matchId;
//         private readonly UdpSendUtils udpSendUtils;
//         private readonly ServerGameContext gameContext;
//         private readonly IGroup<ServerGameEntity> playersGroup;
//         private readonly Dictionary<int, WeaponInfo[]> lastWeaponInfos;
//         
//         public CooldownInfoUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
//         {
//             this.matchId = matchId;
//             this.udpSendUtils = udpSendUtils;
//             gameContext = contexts.serverGame;
//             playersGroup = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
//             lastWeaponInfos = new Dictionary<int, WeaponInfo[]>(10);
//         }
//
//         public void Initialize()
//         {
//             foreach (var player in playersGroup)
//             {
//                 lastWeaponInfos.Add(player.player.id, new WeaponInfo[0]);
//             }
//         }
//
//         public void Execute()
//         {
//             foreach (var ServerGameEntity in playersGroup)
//             {
//                 var playerId = ServerGameEntity.player.id;
//
//                 var abilityCooldown = ServerGameEntity.hasAbility ? ServerGameEntity.ability.cooldown : 0f;
//
//                 WeaponInfo[] weaponInfos;
//                 if (ServerGameEntity.hasSkin)
//                 {
//                     var skin = ServerGameEntity.skin.value;
//                     weaponInfos = ServerGameEntity.GetAllChildrenGameEntities(gameContext, c => c.hasCannon)
//                         .Select(e => new WeaponInfo(skin.Apply(e.cannon.bullet.typeId), e.cannon.cooldown)).ToArray();
//                 }
//                 else
//                 {
//                     weaponInfos = ServerGameEntity.GetAllChildrenGameEntities(gameContext, c => c.hasCannon)
//                         .Select(e => new WeaponInfo(e.cannon.bullet.typeId, e.cannon.cooldown)).ToArray();
//                 }
//
//                 if (weaponInfos.SequenceEqual(lastWeaponInfos[playerId])) continue;
//
//                 lastWeaponInfos[playerId] = weaponInfos;
//                 udpSendUtils.SendCooldownInfo(matchId, playerId, abilityCooldown, weaponInfos);
//             }    
//         }
//     }
// }