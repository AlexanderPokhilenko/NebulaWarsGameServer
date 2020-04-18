using System.Collections.Generic;
using Entitas;

[Game]
public sealed class UpgradesComponent : IComponent
{
    public Dictionary<ActionBonusObject, byte> bonuses;
}