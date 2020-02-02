using System;
using System.Collections.Generic;
using Entitas;

namespace AmoebaBattleServer01.Experimental.GameEngine.Systems
{
    public class PlayerMovementHandlerSystem : ReactiveSystem<InputEntity>
    {
        private readonly GameContext gameContext;

        public PlayerMovementHandlerSystem(Contexts contexts) : base(contexts.input)
        {
            gameContext = contexts.game;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.Movement.Added());
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasMovement && entity.hasPlayer;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var inputEntity in entities)
            {
                var playerJoystickInput = inputEntity.movement.value;
                var playerId = inputEntity.player.GoogleId;

                var gamePlayer = gameContext.GetEntityWithPlayer(playerId);
                
                var newVelocity = playerJoystickInput * gamePlayer.maxVelocity.value;

                if (gamePlayer.hasVelocity)
                {
                    gamePlayer.ReplaceVelocity(newVelocity);
                }
                else
                {
                    gamePlayer.AddVelocity(newVelocity);
                }
            }
        }
    }
}