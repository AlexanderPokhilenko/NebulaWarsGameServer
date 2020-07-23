using Entitas;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using Server.Udp.Sending;
using UnityEngine;

namespace Server.GameEngine.Systems
{
    public class FrameRateSenderSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly GameContext gameContext;
        private readonly IGroup<GameEntity> playersGroup;
        private float sentDeltaTime;
        private const float minimumDeltaToSend = 0.01f;

        public FrameRateSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            gameContext = contexts.game;
            playersGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            sentDeltaTime = ServerTimeConstants.MinDeltaTime;
        }

        public void Execute()
        {
            if(Mathf.Abs(Chronometer.DeltaTime - sentDeltaTime) < minimumDeltaToSend) return;
            sentDeltaTime = Chronometer.DeltaTime;

            foreach (var gameEntity in playersGroup)
            {
                var playerId = gameEntity.player.id;
                udpSendUtils.SendFrameRate(matchId, playerId, sentDeltaTime);
            }
        }
    }
}