using UnityEngine;

public abstract class EntityCreatorObject : ScriptableObject
{
    public abstract GameEntity CreateEntity(GameContext context);

    public GameEntity CreateEntity(GameContext context, Vector2 position, float angle)
    {
        var entity = CreateEntity(context);
        entity.AddPosition(position);
        entity.AddDirection(angle);
        return entity;
    }
}
