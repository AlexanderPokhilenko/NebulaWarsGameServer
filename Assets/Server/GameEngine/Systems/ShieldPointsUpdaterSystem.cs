﻿using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;
using UnityEngine;

namespace Server.GameEngine.Systems
{
    public class ShieldPointsUpdaterSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly Dictionary<int, float> playerMaxShieldPoints;
        private readonly GameContext gameContext;
        private readonly IGroup<GameEntity> players;

        public ShieldPointsUpdaterSystem(Contexts contexts)
        {
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
            }
        }

        public void Execute()
        {
            foreach (var e in players)
            {
                var shield = e.GetAllChildrenGameEntities(gameContext, c => c.hasViewType && c.viewType.id == ViewTypeId.Shield).FirstOrDefault();
                int playerId = e.player.id;
                if (shield == null)
                {
                    if (playerMaxShieldPoints[playerId] >= 0f)
                    {
                        playerMaxShieldPoints[playerId] = 0;
                        UdpSendUtils.SendMaxShieldPoints(playerId, 0f);
                    }
                    continue;
                }

                UdpSendUtils.SendShieldPoints(playerId, shield.healthPoints.value);

                var maxPoints = shield.maxHealthPoints.value;
                if (maxPoints != playerMaxShieldPoints[playerId])
                {
                    playerMaxShieldPoints[playerId] = maxPoints;
                    UdpSendUtils.SendMaxShieldPoints(playerId, maxPoints);
                }
            }
        }
    }
}