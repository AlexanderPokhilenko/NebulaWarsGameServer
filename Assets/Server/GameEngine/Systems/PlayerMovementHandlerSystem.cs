using System.Collections.Generic;
using Entitas;

namespace Server.GameEngine.Systems
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
                var playerId = inputEntity.player.id;

                var gamePlayer = gameContext.GetEntityWithPlayer(playerId);

                if (playerJoystickInput != Vector2.zero)
                {
                    var newVelocity = playerJoystickInput * gamePlayer.maxVelocity.value;
                    if (gamePlayer.hasVelocity)
                    {
                        gamePlayer.ReplaceVelocity(newVelocity);
                    }
                    else
                    {
                        gamePlayer.AddVelocity(newVelocity);
                    }

                    var directionAngle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
                    if (directionAngle < 0) directionAngle += 360f;
                    var deltaAngle = directionAngle - gamePlayer.direction.angle;
                    if (deltaAngle > 180f)
                    {
                        deltaAngle -= 360f;
                    }
                    else if (deltaAngle < -180f)
                    {
                        deltaAngle += 360f;
                    }

                    var deltaAngularVelocity = deltaAngle / Clock.deltaTime;
                    if (gamePlayer.hasAngularVelocity)
                    {
                        gamePlayer.ReplaceAngularVelocity(deltaAngularVelocity);
                    }
                    else
                    {
                        gamePlayer.AddAngularVelocity(deltaAngularVelocity);
                    }
                }
                else
                {
                    if (gamePlayer.hasVelocity) gamePlayer.RemoveVelocity();
                    if (gamePlayer.hasAngularVelocity && !gamePlayer.hasDirectionTargeting) gamePlayer.RemoveAngularVelocity();
                }
            }
        }
    }
}