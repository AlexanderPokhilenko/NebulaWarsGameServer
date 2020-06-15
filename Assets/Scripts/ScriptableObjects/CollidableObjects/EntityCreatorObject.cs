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

    public GameEntity CreateEntity(GameContext context, ushort teamId)
    {
        var entity = CreateEntity(context);
        foreach (var child in entity.GetAllChildrenGameEntities(context))
        {
            child.AddTeam(teamId);
        }
        return entity;
    }

    public GameEntity CreateEntity(GameContext context, Vector2 position, float angle, ushort teamId)
    {
        var entity = CreateEntity(context, position, angle);
        foreach (var child in entity.GetAllChildrenGameEntities(context))
        {
            child.AddTeam(teamId);
        }
        return entity;
    }
}
