namespace Server.GameEngine.Experimental.Systems.Debug
{
    // public class MatchDebugSenderSystem:IExecuteSystem
    // {
    //     private readonly int matchId;
    //     private readonly UdpSendUtils udpSendUtils;
    //     readonly IGroup<GameEntity> playersGroup;
    //     
    //     public MatchDebugSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
    //     {
    //         this.matchId = matchId;
    //         this.udpSendUtils = udpSendUtils;
    //         playersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
    //     }
    //     
    //     public void Execute()
    //     {
    //         foreach (var playerGameEntity in playersGroup)
    //         {
    //             udpSendUtils.SendMatchId(matchId, playerGameEntity .player.id);
    //         }
    //     }
    // }
}