using Entitas;

[Game]
public sealed class DirectionSaverComponent : IComponent
{
    public const float DefaultTime = 2.5f;
    public float angle;
    public float time;
}