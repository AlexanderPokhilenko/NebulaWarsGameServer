using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class ShieldPointsUpdaterSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly Dictionary<int, float> playerMaxShieldPoints;
        private readonly GameContext gameContext;
        private readonly IGroup<GameEntity> players;

        public ShieldPointsUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            gameContext = contexts.game;
            players = gameContext
                .GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            playerMaxShieldPoints = new Dictionary<int, float>(10);
        }

        public void Initialize()
        {
            foreach (var player in players)
            {
                playerMaxShieldPoints.Add(player.player.id, 0f);
                udpSendUtils.SendMaxShieldPoints(matchId, player.player.id, 0f);
            }
        }

        public void Execute()
        {
            foreach (var e in players)
            {
                var shield = e.GetAllChildrenGameEntities(gameContext, c => c.hasViewType && c.viewType.id == ViewTypeId.Shield).FirstOrDefault();
                var playerId = e.player.id;
                if (shield == null)
                {
                    if (playerMaxShieldPoints[playerId] > 0f)
                    {
                        playerMaxShieldPoints[playerId] = 0f;
                        udpSendUtils.SendMaxShieldPoints(matchId, playerId, 0f);
                    }
                    continue;
                }

                udpSendUtils.SendShieldPoints(matchId, playerId, shield.healthPoints.value);

                var maxPoints = shield.maxHealthPoints.value;
                if (Math.Abs(maxPoints - playerMaxShieldPoints[playerId]) > 0.5f)
                {
                    playerMaxShieldPoints[playerId] = maxPoints;
                    udpSendUtils.SendMaxShieldPoints(matchId, playerId, maxPoints);
                }
            }
        }
    }
}