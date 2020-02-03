using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace AmoebaBattleServer01.Experimental.GameEngine.Systems
{
    public class PlayerAttackHandlerSystem : ReactiveSystem<InputEntity>
    {
        private const float attackDelta = 5f;
        private readonly GameContext gameContext;

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

                if (float.IsNaN(playerAttackDirection))
                {
                    if(gamePlayer.hasAngularVelocity) gamePlayer.RemoveAngularVelocity();
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

                var rotatingDelta = playerAttackDirection - gamePlayer.direction.angle;

                if (rotatingDelta > 180f)
                {
                    rotatingDelta -= 360f;
                }
                else if(rotatingDelta < -180f)
                {
                    rotatingDelta += 360f;
                }

                var newAngularVelocity = rotatingDelta / Clock.deltaTime;

                if (gamePlayer.hasAngularVelocity)
                {
                    gamePlayer.ReplaceAngularVelocity(newAngularVelocity);
                }
                else
                {
                    gamePlayer.AddAngularVelocity(newAngularVelocity);
                }
                // сначала мы пытаемся "довернуться", потом выстрелить (если мы почти навелись)
                if (Mathf.Abs(newAngularVelocity) <= attackDelta) gamePlayer.isTryingToShoot = true;
            }
        }
    }
}