using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Каждый кадр (или реже) отправляет всем игрокам дельту (или нет) состояния мира
    /// </summary>
    public class NetworkSenderSystem:IExecuteSystem
    {
        private readonly IGroup<GameEntity> players;
        private readonly IGroup<GameEntity> playersWithHp;
        private readonly IGroup<GameEntity> viewObjects;

        public NetworkSenderSystem(Contexts contexts)
        {
            var gameContext = contexts.game;
            players = gameContext.GetGroup(GameMatcher.Player);
            playersWithHp = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.HealthPoints));
            var viewMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.GlobalTransform, GameMatcher.ViewType);
            viewObjects = gameContext.GetGroup(viewMatcher);
        }
        public void Execute()
        {
            foreach (var player in players)
            {
                UdpSendUtils.SendPositions(player.player.id, viewObjects.AsEnumerable());
            }

            // foreach (var playerWithHp in playersWithHp.AsEnumerable())
            // {
            //     UdpSendUtils.SendHealthPoints(playerWithHp.player.id, playerWithHp.healthPoints.value);
            // }
        }
    }
}