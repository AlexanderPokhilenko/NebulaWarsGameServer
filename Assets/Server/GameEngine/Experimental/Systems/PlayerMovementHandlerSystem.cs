﻿using System.Collections.Generic;
using Entitas;
using log4net;
using UnityEngine;

namespace Server.GameEngine.Systems
{
    public class PlayerMovementHandlerSystem : ReactiveSystem<InputEntity>
    {
        private readonly GameContext gameContext;
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerMovementHandlerSystem));

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

                if (gamePlayer == null)
                {
                    Log.Warn("Пришло сообщение о движении от игрока, которого (уже) нет в комнате. Данные игнорируются.");
                    return;
                }

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
                    if (gamePlayer.hasDirectionTargeting)
                    {
                        gamePlayer.ReplaceDirectionTargeting(directionAngle);
                    }
                    else
                    {
                        gamePlayer.AddDirectionTargeting(directionAngle);
                    }
                    gamePlayer.isDirectionTargetingShooting = false;
                }
                else
                {
                    if (gamePlayer.hasVelocity) gamePlayer.RemoveVelocity();
                    if (!gamePlayer.isDirectionTargetingShooting)
                    {
                        if (gamePlayer.hasDirectionTargeting) gamePlayer.RemoveDirectionTargeting();
                        if(gamePlayer.hasAngularVelocity) gamePlayer.RemoveAngularVelocity();
                    }
                }
            }
        }
    }
}