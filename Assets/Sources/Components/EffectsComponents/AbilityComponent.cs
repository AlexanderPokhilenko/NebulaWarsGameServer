using System;
using Entitas;

[Game]
public sealed class AbilityComponent : IComponent
{
    public float cooldown;
    public Action<GameEntity, GameContext> action;
}