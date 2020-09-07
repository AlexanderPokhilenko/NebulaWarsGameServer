using System;
using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode;
using Plugins.submodules.SharedCode.LagCompensation;
using Plugins.submodules.SharedCode.Logger;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Plugins.submodules.SharedCode.Systems;
using Server.Udp.Sending;
using UnityEngine;

namespace Server.GameEngine.Systems.Sending
{
    /// <summary>
    /// Каждому игроку отправляет все позиции.
    /// </summary>
    public class TransformSenderSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<ServerGameEntity> allWithView;
        private readonly IGroup<ServerGameEntity> alivePlayers;
        private readonly IServerSnapshotHistory serverSnapshotHistory;
        private readonly VectorValidator vectorValidator = new VectorValidator();
        private readonly ILastProcessedInputIdStorage lastProcessedInputIdStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(TransformSenderSystem));

        public TransformSenderSystem(int matchId, Contexts contexts, UdpSendUtils udpSendUtils,
            IServerSnapshotHistory serverSnapshotHistory, ILastProcessedInputIdStorage lastProcessedInputIdStorage)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            this.serverSnapshotHistory = serverSnapshotHistory;
            this.lastProcessedInputIdStorage = lastProcessedInputIdStorage;
            var matcher = ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot);
            alivePlayers = contexts.serverGame.GetGroup(matcher);
            allWithView = contexts.serverGame.GetGroup(ServerGameMatcher.Transform);
        }

        public void Execute()
        {
            try
            {
                var allGos = new Dictionary<ushort, ViewTransformCompressed>();
                if (allWithView.count == 0)
                {
                    log.Error("Нет объектов с моделями");
                    return;
                }
                
                foreach (var entity in allWithView)
                {
                    if (entity.transform.value == null)
                    {
                        throw new Exception("transform пуст id = "+entity.id.value);
                    }
                    
                    Vector3 position = entity.transform.value.position;
                    float x = position.x;
                    float z = position.z;
                    float angle = entity.transform.value.rotation.eulerAngles.y;
                    ViewTypeEnum viewTypeEnum = entity.viewType.value;
                    
                    // log.Debug($"тут x={x} z={z} angle={angle} viewTypeEnum={viewTypeEnum}");
                    ViewTransformCompressed viewTransform = new ViewTransformCompressed(x, z, angle, viewTypeEnum);

                    if (!vectorValidator.TryValidate(viewTransform.GetPosition()))
                    {
                        log.Error("Позиция содержит мусор.");
                        continue;
                    }
                    
                    allGos.Add(entity.id.value, viewTransform);
                }

                if (allGos.Count == 0)
                {
                    log.Error("Пустые координаты");
                    return;
                }
                
                //отправить всем игрокам позиции всех объектов
                foreach (var entity in alivePlayers)
                {
                    int tickNumber = serverSnapshotHistory.GetLastTickNumber();
                    float tickTime = serverSnapshotHistory.GetLastTickTime();
                    if (tickNumber!=0 && Math.Abs(tickTime) < 0.01)
                    {
                        log.Debug($"Пустое время tickNumber = {tickNumber} tickTime = {tickTime}");
                    }

                    ushort playerId = entity.player.tmpPlayerId;
                    uint? lastProcessedInputId = lastProcessedInputIdStorage.Get(playerId);
                    //todo уменьшить это число
                    if (tickNumber > 35 && lastProcessedInputId == null)
                    {
                        log.Info($"Пустой lastProcessedInputId. playerId = {playerId} tickNumber = {tickNumber}");
                    }

                    uint lastProcessedInputIdValue = lastProcessedInputId ?? 0;
                    udpSendUtils.SendPositions(matchId, entity.player.tmpPlayerId, allGos, tickNumber, tickTime,
                        lastProcessedInputIdValue);
                }
            }
            catch (Exception e)
            {
                log.Error(e.FullMessage());   
            }
        }
    }
}