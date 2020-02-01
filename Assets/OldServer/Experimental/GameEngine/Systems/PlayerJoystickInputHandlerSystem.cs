using System;
using System.Collections.Generic;
using Entitas;

namespace AmoebaBattleServer01.Experimental.GameEngine.Systems
{
    /// <summary>
    /// Передвигает позиции игроков по их вводу
    /// </summary>
    public class PlayerJoystickInputHandlerSystem:ReactiveSystem<InputEntity>
    {
        private readonly GameContext gameContext;
        private float stubSpeed = 0.15f;
        
        public PlayerJoystickInputHandlerSystem(Contexts contexts) : base(contexts.input)
        {
            gameContext = contexts.game;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.PlayerJoystickInput.Added());
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasPlayerJoystickInput;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            // Console.WriteLine("PlayerJoystickInputHandlerSystem reactive");
            foreach (var inputEntity in entities)
            {
                var playerJoystickInput = inputEntity.playerJoystickInput;
                if (playerJoystickInput.IsValid())
                {
                    var player = gameContext.GetEntityWithPlayer(playerJoystickInput.PlayerGoogleId);
            
                    if (player == null)
                    {
                        Console.WriteLine("player null");
                        continue;
                    }
            
                    if (player.position == null)
                    {
                        Console.WriteLine("player position null");
                        continue;
                    }
                    
                    player.position.X += stubSpeed * playerJoystickInput.X;
                    player.position.Y += stubSpeed * playerJoystickInput.Y;
                }
            }
        }
    }
}