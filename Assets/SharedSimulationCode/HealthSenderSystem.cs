using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Каждый кадр отправляет значение hp для всех игроков
    /// </summary>
    public class HealthSenderSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly IGroup<GameEntity> withHp;
        private readonly IGroup<GameEntity> players;
        private readonly IHealthPointsPackSender healthPointsPackSender;

        public HealthSenderSystem(Contexts contexts, int matchId, IHealthPointsPackSender healthPointsPackSender)
        {
            this.matchId = matchId;
            this.healthPointsPackSender = healthPointsPackSender;
            
            var gameContext = contexts.game;
            withHp = gameContext.GetGroup(GameMatcher.HealthPoints);
            
            players = gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.Player)
                .NoneOf(GameMatcher.Bot));
        }
        
        public void Execute()
        {
            //todo найти hp, которые изменились
            Dictionary<ushort, float> entityIdToHealthValue = new Dictionary<ushort, float>();

            foreach (var entity in withHp)
            {
                ushort entityId = entity.id.value;
                float currentValue = entity.healthPoints.value;
                entityIdToHealthValue.Add(entityId, currentValue);
            }

            if (entityIdToHealthValue.Count == 0)
            {
                Debug.LogWarning("Отправка hp не происходит.");
                return;
            }
            
            foreach (var entity in players)
            {
                ushort tmpPlayerId = entity.player.id;
                healthPointsPackSender.SendHealthPointsPack(matchId, tmpPlayerId, entityIdToHealthValue);
            }
        }
    }
}