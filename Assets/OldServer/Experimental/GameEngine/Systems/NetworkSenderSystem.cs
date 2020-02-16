using Entitas;
using OldServer.Experimental.Udp.Sending;

namespace OldServer.Experimental.GameEngine.Systems
{
    /// <summary>
    /// Каждый кадр (или реже) отправляет всем игрокам дельту (или нет) состояния мира
    /// </summary>
    public class NetworkSenderSystem:IExecuteSystem
    {
        private readonly GameContext gameContext;

        public NetworkSenderSystem(Contexts contexts)
        {
            gameContext = contexts.game;
        }
        public void Execute()
        {
            var players = gameContext.GetEntities(Matcher<GameEntity>.AllOf(GameMatcher.Player));
            var withPosition = gameContext.GetEntities(Matcher<GameEntity>.AllOf(GameMatcher.Position));
            foreach (var player in players)
            {
                UdpSendUtils.SendPositions(player.player.id, withPosition);
            }
        }
    }
}