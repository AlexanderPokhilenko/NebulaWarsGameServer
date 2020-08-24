// using Entitas;
// using Plugins.submodules.SharedCode.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
// using Server.GameEngine.Chronometers;
// using Server.Udp.Sending;
// using UnityEngine;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     public class FrameRateSenderSystem : IExecuteSystem
//     {
//         private readonly int matchId;
//         private readonly UdpSendUtils udpSendUtils;
//         private readonly ServerGameContext gameContext;
//         private readonly IGroup<ServerGameEntity> playersGroup;
//         private float sentDeltaTime;
//         private const float minimumDeltaToSend = 0.01f;
//
//         public FrameRateSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
//         {
//             this.matchId = matchId;
//             this.udpSendUtils = udpSendUtils;
//             gameContext = contexts.serverGame;
//             playersGroup = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
//             sentDeltaTime = ServerTimeConstants.MinDeltaTime;
//         }
//
//         public void Execute()
//         {
//             if(Mathf.Abs(Chronometer.DeltaTime - sentDeltaTime) < minimumDeltaToSend) return;
//             sentDeltaTime = Chronometer.DeltaTime;
//
//             foreach (var ServerGameEntity in playersGroup)
//             {
//                 var playerId = ServerGameEntity.player.id;
//                 udpSendUtils.SendFrameRate(matchId, playerId, sentDeltaTime);
//             }
//         }
//     }
// }