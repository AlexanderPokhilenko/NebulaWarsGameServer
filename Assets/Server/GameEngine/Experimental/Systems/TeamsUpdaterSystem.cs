using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class TeamsUpdaterSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> playersGroup;

        public TeamsUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.game)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersGroup = contexts.game
                .GetGroup(GameMatcher
                    .AllOf(GameMatcher.Player)
                    .NoneOf(GameMatcher.Bot));
        }

        public void Initialize()
        {
            var teams = new Dictionary<ushort, byte>(playersGroup.count);

            foreach (var gameEntity in playersGroup)
            {
                teams.Add(gameEntity.id.value, gameEntity.team.id);
            }

            foreach (var gameEntity in playersGroup)
            {
                udpSendUtils.SendTeams(matchId, gameEntity.player.id, teams);
            }
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Team);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasTeam && !entity.isDestroyed && entity.hasViewType && entity.hasHealthPoints;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var teams = new Dictionary<ushort, byte>(entities.Count);

            foreach (var gameEntity in entities)
            {
                teams.Add(gameEntity.id.value, gameEntity.team.id);
            }

            foreach (var player in playersGroup)
            {
                udpSendUtils.SendTeams(matchId, player.player.id, teams);
            }
        }
    }
}