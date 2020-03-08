using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;

using UnityEngine;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Каждый кадр (или реже) отправляет всем игрокам дельту (или нет) состояния мира
    /// </summary>
    public class NetworkSenderSystem:IExecuteSystem,IInitializeSystem
    {
        private readonly IGroup<GameEntity> players;
        private readonly IGroup<GameEntity> playersWithHp;
        private readonly IGroup<GameEntity> viewObjects;
        private GameEntity zone;
        private readonly GameContext gameContext;

        public NetworkSenderSystem(Contexts contexts)
        {
            gameContext = contexts.game;
            players = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            playersWithHp = 
                gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.HealthPoints).NoneOf(GameMatcher.Bot));
            var viewMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.GlobalTransform, GameMatcher.ViewType);
            viewObjects = gameContext.GetGroup(viewMatcher);
        }
        
        public void Initialize()
        {
            zone = gameContext.zone.GetZone(gameContext);
        }
        
        public void Execute()
        {
            foreach (var player in players)
            {
                var visibleObjects = GetVisibleObjects(player);
                UdpSendUtils.SendPositions(player.player.id, visibleObjects);
            }

            foreach (var playerWithHp in playersWithHp)
            {
                UdpSendUtils.SendHealthPoints(playerWithHp.player.id, playerWithHp.healthPoints.value);
            }
        }
        
        private HashSet<GameEntity> GetVisibleObjects(GameEntity currentPlayer)
        {
            HashSet<GameEntity> result = new HashSet<GameEntity>();
            Vector2 currentPlayerPosition = currentPlayer.globalTransform.position;
            const float sqrVisibleAreaRadius = 13*13;
            
            // if (currentPlayer.player.id == 888_777) Log.Info("current position = " + position.x + " " + position.y);
            
            foreach (var withView in viewObjects)
            {
                if ((withView.globalTransform.position - currentPlayerPosition).sqrMagnitude < sqrVisibleAreaRadius)
                {
                    result.Add(withView);
                }
            }

            result.Add(zone);
            
            return result;
        }
    }
}