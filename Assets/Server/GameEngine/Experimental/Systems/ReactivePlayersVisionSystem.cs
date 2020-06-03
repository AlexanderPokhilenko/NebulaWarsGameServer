using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public abstract class ReactivePlayersVisionSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly GameContext gameContext;
        private readonly IGroup<GameEntity> players;
        private const float visibleAreaRadius = 15;
        private GameEntity zone;

        protected ReactivePlayersVisionSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.game)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            gameContext = contexts.game;
            players = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
        }

        public void Initialize()
        {
            zone = gameContext.zone.GetZone(gameContext);
        }

        protected abstract void SendData(UdpSendUtils udpSendUtils, int matchId, int playerId, IEnumerable<GameEntity> entities);

        protected sealed override void Execute(List<GameEntity> entities)
        {
            if (zone.circleCollider.radius > visibleAreaRadius)
            {
                foreach (var player in players)
                {
                    var playerVisibleObjects = GetVisibleObjects(player, entities);
                    if(playerVisibleObjects.Count == 0) continue;
                    SendData(udpSendUtils, matchId, player.player.id, playerVisibleObjects);
                }
            }
            else
            {
                foreach (var player in players)
                {
                    SendData(udpSendUtils, matchId, player.player.id, entities);
                }
            }
        }

        private static List<GameEntity> GetVisibleObjects(GameEntity currentPlayer, IEnumerable<GameEntity> entities)
        {
            var result = new List<GameEntity>();
            var currentPlayerPosition = currentPlayer.globalTransform.position;

            foreach (var withView in entities)
            {
                AddViewObject(withView);
            }

            return result;

            void AddViewObject(GameEntity e)
            {
                var radius = e.hasCircleCollider ? e.circleCollider.radius : 0;
                if ((e.globalTransform.position - currentPlayerPosition).magnitude - radius <= visibleAreaRadius)
                {
                    result.Add(e);
                }
            }
        }
    }
}