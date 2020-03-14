using System;
using Entitas;

[Game]
public sealed class ActionBonusComponent : IComponent
{
    public Action<GameEntity> action;
}
