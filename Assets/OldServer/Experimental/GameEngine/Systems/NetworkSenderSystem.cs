using System;
using AmoebaBattleServer01.Experimental.Udp;
using Entitas;

namespace AmoebaBattleServer01.Experimental.GameEngine.Systems
{
    /// <summary>
    /// Каждый кадр (или реже) отправляет всем игрокам дельту (или нет) состояния мира
    /// </summary>
    public class NetworkSenderSystem:IExecuteSystem
    {
        private readonly GameContext gameContext;
        private readonly IGroup<GameEntity> players;
        private readonly IGroup<GameEntity> viewObjects;

        public NetworkSenderSystem(Contexts contexts)
        {
            gameContext = contexts.game;
            players = gameContext.GetGroup(GameMatcher.Player);
            var viewMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.GlobalTransform, GameMatcher.ViewType);
            viewObjects = gameContext.GetGroup(viewMatcher);
        }
        public void Execute()
        {
            foreach (var player in players)
            {
                UdpSendUtils.SendPositions(player.player.id, viewObjects.AsEnumerable());
            }
        }
    }
}