using Entitas;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.Udp.Sending;
using System.Collections.Generic;
using System.Linq;

namespace Server.GameEngine.Systems
{
    //TODO: Переписать!
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

        public PositionsSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            gameContext = contexts.game;
            players = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            var grandMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction).NoneOf(GameMatcher.Parent);
            grandObjects = gameContext.GetGroup(grandMatcher);
            var visibleMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.ViewType);
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
                    var playerId = player.player.id;
                    var playerVisible = GetVisible(player);
                    udpSendUtils.SendPositions(matchId, playerId, playerVisible);
                }
            }
            else
            {
                var visibleDict = visibleObjects.GetEntities(visibleObjectsBuffer)
                    .ToDictionary(e => e.id.value,
                        e => new ViewTransform(e.position.value,
                            e.direction.angle,
                            e.viewType.id));
                foreach (var player in players)
                {
                    udpSendUtils.SendPositions(matchId, player.player.id, visibleDict);
                }
            }
        }
        
        private Dictionary<ushort, ViewTransform> GetVisible(GameEntity currentPlayer)
        {
            var result = new Dictionary<ushort, ViewTransform>();
            var currentPlayerPosition = currentPlayer.globalTransform.position;
            
            foreach (var withView in grandObjects)
            {
                AddViewObject(withView);
            }
            
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
                        if(result.ContainsKey(child.id.value)) continue;
                        result.Add(child.id.value, new ViewTransform(child.position.value, child.direction.angle, child.viewType.id));
                    }
                }
            }
        }
    }
}