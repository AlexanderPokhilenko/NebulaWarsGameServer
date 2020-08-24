using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode.Logger;
using Server.GameEngine.MatchLifecycle;

namespace Server.GameEngine.Experimental.Systems
{
    /// <summary>
    /// Вызывает удаление матча, когда остаётся мало игроков
    /// </summary>
    public class FinishMatchSystem:ReactiveSystem<ServerGameEntity>
    {
        private readonly int matchId;
        private readonly MatchRemover matchRemover;
        private readonly IGroup<ServerGameEntity> alivePlayersGroup;
        private readonly IGroup<ServerGameEntity> alivePlayersAndBotsGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(FinishMatchSystem));
        
        public FinishMatchSystem(Contexts contexts, MatchRemover matchRemover, int matchId) 
            : base(contexts.serverGame)
        {
            alivePlayersGroup = contexts.serverGame.GetGroup(
                ServerGameMatcher.AllOf(ServerGameMatcher.Player).
                    NoneOf(ServerGameMatcher.KilledBy, ServerGameMatcher.Bot));
            
            alivePlayersAndBotsGroup = contexts.serverGame.GetGroup(
                ServerGameMatcher.AllOf(ServerGameMatcher.Player).
                    NoneOf(ServerGameMatcher.KilledBy));
            
            this.matchRemover = matchRemover;
            this.matchId = matchId;
        }

        protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
        {
            return context.CreateCollector(ServerGameMatcher.KilledBy.Added());
        }

        protected override bool Filter(ServerGameEntity entity)
        {
            return entity.hasKilledBy && entity.hasPlayer;
        }

        protected override void Execute(List<ServerGameEntity> entities)
        {
            LogKilledEntities(entities);

            switch (alivePlayersAndBotsGroup.count)
            {
                case 0:
                    //последние игроки погибли в одном кадре
                    matchRemover.MarkMatchAsFinished(matchId);
                    break;
                case 1 :
                    //есть победитель
                    matchRemover.MarkMatchAsFinished(matchId);
                    break;
            }

            if (alivePlayersGroup.count == 0)
            {
                //живых игроков не осталось. остались только боты
                //чем закончится драка ботов неинтересно матч можно завершать
                log.Info("Живые игроки в матче кончились.");
                matchRemover.MarkMatchAsFinished(matchId);
            }
        }

        private void LogKilledEntities(List<ServerGameEntity> entities)
        {
            log.Info($" {nameof(matchId)} {matchId} Погибло игровых сущностей: {entities.Count}. ");
            foreach (var ServerGameEntity in entities)
            {
                if (ServerGameEntity.isBot)
                {
                    log.Info($"{nameof(matchId)} {matchId} Погиб бот {ServerGameEntity.viewType.value}");
                }
                else if(ServerGameEntity.hasPlayer)
                {
                    log.Info($"{nameof(matchId)} {matchId} Погиб игрок {ServerGameEntity.player.id}");
                }
                else
                {
                    log.Error("Был убит неопознанный объект");
                }
            }
        }
    }
}