using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.Udp.Sending;
using SharedSimulationCode.LagCompensation;
using UnityEngine;

namespace SharedSimulationCode.Systems.Sending
{
    public interface ITickNumberStorage
    {
        int GetTickNumber();
    }
    /// <summary>
    /// Кажному игроку отправляет все позиции.
    /// </summary>
    public class TransformSenderSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> allWithView;
        private readonly IGroup<GameEntity> alivePlayers;
        private readonly IGameStateHistory gameStateHistory;

        public TransformSenderSystem(int matchId, Contexts contexts, UdpSendUtils udpSendUtils,
            IGameStateHistory gameStateHistory)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            this.gameStateHistory = gameStateHistory;
            alivePlayers = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            allWithView = contexts.game.GetGroup(GameMatcher.Transform);
        }

        public void Execute()
        {
            Dictionary<ushort, ViewTransform> allGos = new Dictionary<ushort, ViewTransform>();

            if (allWithView.count == 0)
            {
                Debug.LogError("Нет объектов с моделями");
                return;
            }
            foreach (var entity in allWithView)
            {
                var position = entity.transform.value.position;
                float x = position.x;
                float z = position.z;
                float angle = entity.transform.value.rotation.eulerAngles.y;
                ViewTypeId viewTypeId = entity.viewType.id;
                ViewTransform viewTransform = new ViewTransform(x, z, angle, viewTypeId);
                allGos.Add(entity.id.value, viewTransform);
            }

            if (allGos.Count == 0)
            {
                Debug.LogError("Пустые координаты");
                return;
            }
            
            //отправить всем игрокам позиции всех объектов
            foreach (var entity in alivePlayers)
            {
                udpSendUtils.SendPositions(matchId, entity.player.id, allGos, gameStateHistory.GetLastTickNumber());
            }
        }
    }
}