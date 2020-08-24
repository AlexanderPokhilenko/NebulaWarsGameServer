namespace Server.GameEngine.Experimental.Systems.Debug
{
    // public class MatchDebugSenderSystem:IExecuteSystem
    // {
    //     private readonly int matchId;
    //     private readonly UdpSendUtils udpSendUtils;
    //     readonly IGroup<ServerGameEntity> playersGroup;
    //     
    //     public MatchDebugSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
    //     {
    //         this.matchId = matchId;
    //         this.udpSendUtils = udpSendUtils;
    //         playersGroup = contexts.serverGame.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
    //     }
    //     
    //     public void Execute()
    //     {
    //         foreach (var playerServerGameEntity in playersGroup)
    //         {
    //             udpSendUtils.SendMatchId(matchId, playerServerGameEntity .player.id);
    //         }
    //     }
    // }
}