// using System.Collections.Generic;
//
// public abstract class SpecialShooter
// {
//     private readonly List<ServerGameEntity> cannonsBuffer = new List<ServerGameEntity>();
//
//     protected abstract IEnumerable<ServerGameEntity> GetSpecialCannons(List<ServerGameEntity> cannons);
//
//     public IEnumerable<ServerGameEntity> GetCannons(ServerGameEntity entity, ServerGameContext gameContext)
//     {
//         cannonsBuffer.Clear();
//
//         FillBuffer(entity, gameContext);
//
//         return GetSpecialCannons(cannonsBuffer);
//     }
//
//     private void FillBuffer(ServerGameEntity entity, ServerGameContext gameContext)
//     {
//         if (entity.hasCannon) cannonsBuffer.Add(entity);
//
//         foreach (var child in gameContext.GetEntitiesWithParent(entity.id.value))
//         {
//             if (child.hasSpecialShooter)
//             {
//                 cannonsBuffer.AddRange(child.specialShooter.value.GetCannons(child, gameContext));
//             }
//             else
//             {
//                 FillBuffer(child, gameContext);
//             }
//         }
//     }
// }
