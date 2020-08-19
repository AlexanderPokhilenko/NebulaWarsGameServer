using System.Collections.Generic;
using Code.Common.Logger;
using Entitas;

namespace Server.GameEngine.Experimental.Systems
{
    public class PlayerAttackHandlerSystem : ReactiveSystem<InputEntity>
    {
        private readonly GameContext gameContext;
        private static readonly ILog Log = LogManager.CreateLogger(typeof(PlayerAttackHandlerSystem));
        
        public PlayerAttackHandlerSystem(Contexts contexts) : base(contexts.input)
        {
            gameContext = contexts.game;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.Attack.Added());
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasAttack && entity.hasPlayer;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var inputEntity in entities)
            {
                var playerAttackDirection = inputEntity.attack.direction;

                var playerId = inputEntity.player.id;

                var gamePlayer = gameContext.GetEntityWithPlayer(playerId);

                if (gamePlayer == null)
                {
                    Log.Warn("Пришло сообщение об атаке от игрока, которого (уже) нет в комнате. Данные игнорируются.");
                    return;
                }

                if (float.IsNaN(playerAttackDirection))
                {
                    //if(gamePlayer.hasDirectionTargeting) gamePlayer.RemoveDirectionTargeting();
                    gamePlayer.isDirectionTargetingShooting = false;
                    continue;
                }

                if (playerAttackDirection < 0f)
                {
                    if (playerAttackDirection <= -360f)
                    {
                        playerAttackDirection %= 360;
                    }
                    playerAttackDirection += 360;
                }
                else if(playerAttackDirection >= 360f)
                {
                    playerAttackDirection %= 360;
                }

                gamePlayer.ReplaceDirectionTargeting(playerAttackDirection);
                gamePlayer.ReplaceDirectionSaver(playerAttackDirection, DirectionSaverComponent.DefaultTime * 0.5f);

                gamePlayer.isDirectionTargetingShooting = true;
            }
        }
    }
}