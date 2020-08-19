using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode.Logger;


namespace Server.GameEngine.Systems
{
    public class PlayerAbilityHandlerSystem : ReactiveSystem<InputEntity>
    {
        private readonly GameContext gameContext;
        private readonly ILog log = LogManager.CreateLogger(typeof(PlayerAbilityHandlerSystem));
        
        public PlayerAbilityHandlerSystem(Contexts contexts) : base(contexts.input)
        {
            gameContext = contexts.game;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.TryingToUseAbility.Added());
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isTryingToUseAbility && entity.hasPlayer;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var inputEntity in entities)
            {
                var playerId = inputEntity.player.id;

                var gamePlayer = gameContext.GetEntityWithPlayer(playerId);

                if (gamePlayer == null)
                {
                    log.Warn("Пришло сообщение о способности от игрока, которого (уже) нет в комнате. Данные игнорируются.");
                    return;
                }

                gamePlayer.isTryingToUseAbility = true;
            }
        }
    }
}