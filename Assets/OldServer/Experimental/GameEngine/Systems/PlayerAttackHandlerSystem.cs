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
                var playerId = inputEntity.player.PlayerId;

                // var gamePlayer = gameContext.GetEntityWithPlayer(playerId);
                var gamePlayer = gameContext.GetEntityWithPlayerPlayerId(playerId);
                
                var newAngularVelocity = (playerAttackDirection - gamePlayer.direction.angle) / Time.deltaTime;

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