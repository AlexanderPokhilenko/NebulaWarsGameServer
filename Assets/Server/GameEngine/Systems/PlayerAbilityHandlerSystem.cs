using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode.Logger;


namespace Server.GameEngine.Systems
{
    public class PlayerAbilityHandlerSystem : ReactiveSystem<ServerInputEntity>
    {
        private readonly ServerGameContext gameContext;
        private readonly ILog log = LogManager.CreateLogger(typeof(PlayerAbilityHandlerSystem));
        
        public PlayerAbilityHandlerSystem(Contexts contexts) : base(contexts.serverInput)
        {
            gameContext = contexts.serverGame;
        }

        protected override ICollector<ServerInputEntity> GetTrigger(IContext<ServerInputEntity> context)
        {
            return context.CreateCollector(ServerInputMatcher.TryingToUseAbility.Added());
        }

        protected override bool Filter(ServerInputEntity entity)
        {
            return entity.isTryingToUseAbility && entity.hasPlayerInput;
        }

        protected override void Execute(List<ServerInputEntity> entities)
        {
            foreach (var inputEntity in entities)
            {
                ushort playerId = inputEntity.playerInput.playerEntityId;
                ServerGameEntity gamePlayer = gameContext.GetEntityWithAccount(playerId);
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