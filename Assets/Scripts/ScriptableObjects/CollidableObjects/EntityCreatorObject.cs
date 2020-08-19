// using UnityEngine;
//
// public abstract class EntityCreatorObject : ScriptableObject
// {
//     public abstract void FillEntity(GameContext context, GameEntity entity);
//
//     public void RefillEntity(GameContext context, GameEntity entity, Vector2 position, float direction)
//     {
//         var id = entity.id.value;
//
//         entity.RemoveAllComponents();
//         //entity.RemoveAllOnEntityReleasedHandlers();
//
//         entity.AddId(id);
//         entity.AddPosition(position);
//         entity.AddDirection(direction);
//
//         FillEntity(context, entity);
//     }
//
//     public GameEntity CreateEntity(GameContext context)
//     {
//         var entity = context.CreateEntity();
//         FillEntity(context, entity);
//         return entity;
//     }
//
//     public GameEntity CreateEntity(GameContext context, Vector2 position, float angle)
//     {
//         var entity = CreateEntity(context);
//         entity.AddPosition(position);
//         entity.AddDirection(angle);
//         if (entity.hasInitialDirectionSaver) entity.ReplaceInitialDirectionSaver(angle);
//         return entity;
//     }
//
//     public GameEntity CreateEntity(GameContext context, byte teamId)
//     {
//         var entity = CreateEntity(context);
//         foreach (var child in entity.GetAllChildrenGameEntities(context))
//         {
//             child.AddTeam(teamId);
//         }
//         return entity;
//     }
//
//     public GameEntity CreateEntity(GameContext context, Vector2 position, float angle, byte teamId)
//     {
//         var entity = CreateEntity(context, position, angle);
//         foreach (var child in entity.GetAllChildrenGameEntities(context))
//         {
//             child.AddTeam(teamId);
//         }
//         return entity;
//     }
// }
