using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class MaxHpUpdaterSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly int matchId;
        private readonly GameContext gameContext;
        private readonly PlayersViewAreas viewAreas;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> withMaxHp;
        private readonly Dictionary<ushort, Dictionary<ushort, ushort>> knownMaxHps;

        public MaxHpUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas playersViewAreas)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            viewAreas = playersViewAreas;
            gameContext = contexts.game;
            withMaxHp = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.MaxHealthPoints, GameMatcher.ViewType));
            knownMaxHps = new Dictionary<ushort, Dictionary<ushort, ushort>>(playersViewAreas.Count);
        }

        public void Initialize()
        {
            var allPlayers =
                gameContext.GetEntities(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.MaxHealthPoints));

            var allPlayersMaxHps = new Dictionary<ushort, ushort>(allPlayers.Length);
            foreach (var player in allPlayers)
            {
                allPlayersMaxHps.Add(player.id.value, (ushort)player.maxHealthPoints.value);
            }

            foreach (var pair in viewAreas)
            {
                var playerId = pair.Key;
                knownMaxHps.Add(playerId, new Dictionary<ushort, ushort>(allPlayersMaxHps));
                udpSendUtils.SendMaxHealthPoints(matchId, playerId, allPlayersMaxHps);
            }
        }

        public void Execute()
        {
            foreach (var pair in viewAreas)
            {
                var playerId = pair.Key;
                var currentKnownMaxHps = knownMaxHps[playerId];
                var visible = ReactivePlayersVisionSystem.GetVisibleObjects(viewAreas, pair.Value, withMaxHp.AsEnumerable());

                var changed = new Dictionary<ushort, ushort>(visible.Count);
                foreach (var gameEntity in visible)
                {
                    var id = gameEntity.id.value;
                    var maxHp = (ushort)gameEntity.maxHealthPoints.value;
                    if(currentKnownMaxHps.TryGetValue(id, out var knownMaxHp) && knownMaxHp == maxHp) continue;
                    changed.Add(id, maxHp);
                    currentKnownMaxHps[id] = maxHp;
                }
                //TODO: Не отправлять значения по умолчанию
                if(changed.Count == 0) continue;
                udpSendUtils.SendMaxHealthPoints(matchId, playerId, changed);
            }
        }
    }
}