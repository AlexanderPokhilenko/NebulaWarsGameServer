using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;
using UnityEngine;

namespace Server.GameEngine.Systems.Sending
{
    /// <summary>
    /// Каждый кадр отправляет значение hp для всех игроков
    /// </summary>
    public class MaxHealthSenderSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly IGroup<ServerGameEntity> players;
        private readonly IGroup<ServerGameEntity> withMaxHp;
        private readonly IMaxHealthPointsPackSender maxHealthPointsPackSender;

        public MaxHealthSenderSystem(Contexts contexts, int matchId, 
            IMaxHealthPointsPackSender maxHealthPointsPackSender)
        {
            this.matchId = matchId;
            this.maxHealthPointsPackSender = maxHealthPointsPackSender;
            
            var gameContext = contexts.serverGame;
            withMaxHp = gameContext.GetGroup(ServerGameMatcher.MaxHealthPoints);
            
            players = gameContext.GetGroup(ServerGameMatcher
                .AllOf(ServerGameMatcher.Player)
                .NoneOf(ServerGameMatcher.Bot));
        }
        
        public void Execute()
        {
            //todo найти hp, которые изменились
            Dictionary<ushort, float> entityIdToValue = new Dictionary<ushort, float>();

            foreach (var entity in withMaxHp)
            {
                ushort entityId = entity.id.value;
                float currentValue = entity.maxHealthPoints.value;
                entityIdToValue.Add(entityId, currentValue);
            }

            if (entityIdToValue.Count == 0)
            {
                Debug.LogWarning("Отправка max hp не происходит.");
                return;
            }
            
            foreach (var entity in players)
            {
                ushort tmpPlayerId = entity.player.playerId;
                maxHealthPointsPackSender.SendMaxHealthPointsPack(matchId, tmpPlayerId, entityIdToValue);
            }
        }
    }
}