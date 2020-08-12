using System.Collections.Generic;
using Entitas;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.Udp.Sending;

namespace SharedSimulationCode
{
    /// <summary>
    /// Кажному игроку отправляет все позиции.
    /// </summary>
    public class PositionSenderSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> allWithView;
        private readonly IGroup<GameEntity> alivePlayers;

        public PositionSenderSystem(int matchId, Contexts contexts, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            alivePlayers = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            allWithView = contexts.game
                .GetGroup(GameMatcher.AllOf(GameMatcher.Transform, GameMatcher.ViewType));
        }

        public void Execute()
        {
            Dictionary<ushort, ViewTransform> allGos = new Dictionary<ushort, ViewTransform>();
            foreach (var entity in allWithView)
            {
                var position = entity.transform.value.position;
                float x = position.x;
                float y = position.z;
                float angle = entity.transform.value.rotation.y;
                ViewTypeId viewTypeId = entity.viewType.id;
                ViewTransform viewTransform = new ViewTransform(x, y, angle, viewTypeId);
                allGos.Add(entity.id.value, viewTransform);
            }
            
            //отправить всем игрокам позиции всех объектов
            foreach (var entity in alivePlayers)
            {
                udpSendUtils.SendPositions(matchId, entity.player.id, allGos);
            }
        }
    }
}