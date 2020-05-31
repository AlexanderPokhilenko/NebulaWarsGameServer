using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;
using UnityEngine;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Каждый кадр (или реже) отправляет всем игрокам дельту (или нет) состояния мира
    /// </summary>
    public class PositionsSenderSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> players;
        private readonly IGroup<GameEntity> grandObjects;
        private readonly IGroup<GameEntity> visibleObjects;
        private GameEntity zone;
        private readonly GameContext gameContext;
        private readonly List<GameEntity> visibleObjectsBuffer;
        private const float visibleAreaRadius = 15;
        //private const float sqrVisibleAreaRadius = visibleAreaRadius * visibleAreaRadius;

        public PositionsSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            gameContext = contexts.game;
            players = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            var grandMatcher = GameMatcher.AllOf(GameMatcher.GlobalTransform).NoneOf(GameMatcher.Parent);
            grandObjects = gameContext.GetGroup(grandMatcher);
            var visibleMatcher = GameMatcher.AllOf(GameMatcher.GlobalTransform, GameMatcher.ViewType);
            visibleObjects = gameContext.GetGroup(visibleMatcher);
            visibleObjectsBuffer = new List<GameEntity>();
        }
        
        public void Initialize()
        {
            zone = gameContext.zone.GetZone(gameContext);
        }
        
        public void Execute()
        {
            if (zone.circleCollider.radius > visibleAreaRadius)
            {
                foreach (var player in players)
                {
                    var playerVisibleObjects = GetVisibleObjects(player);
                    udpSendUtils.SendPositions(matchId, player.player.id, playerVisibleObjects);
                }
            }
            else
            {
                var enumerableVisibleObjects = visibleObjects.GetEntities(visibleObjectsBuffer);
                foreach (var player in players)
                {
                    udpSendUtils.SendPositions(matchId, player.player.id, enumerableVisibleObjects);
                }
            }
        }
        
        private IEnumerable<GameEntity> GetVisibleObjects(GameEntity currentPlayer)
        {
            HashSet<GameEntity> result = new HashSet<GameEntity>();
            Vector2 currentPlayerPosition = currentPlayer.globalTransform.position;
            
            foreach (var withView in grandObjects)
            {
                AddViewObject(withView);
            }

            result.Add(zone);
            
            return result;

            void AddViewObject(GameEntity e)
            {
                var radius = e.hasCircleCollider ? e.circleCollider.radius : 0;
                //var sqrRadius = radius * radius;
                if ((e.globalTransform.position - currentPlayerPosition).magnitude - radius <= visibleAreaRadius)
                {
                    var viewChildren = e.GetAllChildrenGameEntities(gameContext, c => c.hasGlobalTransform &&
                        c.hasViewType);
                    foreach (var child in viewChildren)
                    {
                        result.Add(child);
                    }
                }
            }
        }
    }
}