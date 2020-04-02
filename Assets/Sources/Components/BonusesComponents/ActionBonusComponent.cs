using System;
using Entitas;

[Game]
public sealed class ActionBonusComponent : IComponent
{
    public Func<GameEntity, bool> check;
    public Action<GameEntity> action;
}
