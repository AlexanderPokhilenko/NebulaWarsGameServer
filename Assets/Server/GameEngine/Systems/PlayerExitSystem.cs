using System;
using System.Collections.Generic;
using Entitas;
using log4net;
using Server.Http;

namespace Server.GameEngine.Systems
{
    public class PlayerExitSystem:ReactiveSystem<InputEntity>, ICleanupSystem
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitSystem));
    
        private readonly IGroup<GameEntity> alivePlayerAndBotsGroup;
        private readonly IGroup<InputEntity> playerExitGroup;
        private readonly int matchId;
        private readonly MatchStorageFacade matchStorageFacade;

        public PlayerExitSystem(Contexts contexts, int matchId, MatchStorageFacade matchStorageFacade):base(contexts.input)
        {
            var gameContext = contexts.game;
            alivePlayerAndBotsGroup = gameContext.GetGroup(GameMatcher.Player);
            playerExitGroup = contexts.input.GetGroup(InputMatcher.PlayerExit);
            this.matchId = matchId;
            this.matchStorageFacade = matchStorageFacade;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.PlayerExit.Added());
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasPlayerExit;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            Log.Warn("Вызов реактивной системы для преждевременном удалении игрока из матча");
            foreach (var inputEntity in entities)
            {
                int playerId = inputEntity.playerExit.PlayerId;
                Log.Warn($"преждевременное удаление игрока из матча {nameof(playerId)} {playerId}");
                //TODO Пометить сущность игрока для передачи управления в AI системы.
                //TODO удалить нахуй игрока из структуры данных текущего матча
                bool success = matchStorageFacade.TryRemovePlayer(matchId, playerId);
                if (!success)
                {
                    throw new Exception("Сука блять какого хуя ты упал, уёбок?!");
                }
                SendPlayerDeathMessageToMatchmaker(playerId);
            }
        }

        private void SendPlayerDeathMessageToMatchmaker(int playerId)
        {
            int placeInBattle = alivePlayerAndBotsGroup.count;
            PlayerDeathData playerDeathData = new PlayerDeathData
            {
                PlayerId = playerId,
                PlaceInBattle = placeInBattle,
                MatchId = matchId 
            };
            PlayerDeathNotifier.KilledPlayers.Enqueue(playerDeathData);
        }

        public void Cleanup()
        {
            foreach (var inputEntity in playerExitGroup.GetEntities())
            {
                //TODO это нормальная очистка?
                if (inputEntity.hasPlayerExit)
                {
                    inputEntity.RemovePlayerExit();
                }
            }
        }
    }
}