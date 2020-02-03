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
                UdpSendUtils.SendPositions(player.player.PlayerId, withPosition);
            }
        }
    }
}