// using System;
// using NUnit.Framework;
//
// // public class ContextCopyFactory
// // {
// //     public ServerGameContext Create(ServerGameContext original)
// //     {
// //         ServerGameContext copy = new ServerGameContext();
// //         foreach (var ServerGameEntity in original.GetEntities())
// //         {
// //             ServerGameEntity ServerGameEntityCopy = new ServerGameEntity();
// //             ServerGameEntity.CopyPublicMemberValues(ServerGameEntityCopy);
// //         }
// //
// //         return copy;
// //     }
// // }
// namespace Tests
// {
//    
//     public class ReflectionTest
//     {
//         [Test]
//         public void ReflectionTestSimplePasses()
//         {
//             var gameContext = new ServerGameContext();
//             ServerGameEntity entity = gameContext.CreateEntity();
//             entity.AddDamage(84);
//
//             var factory = new ContextCopyFactory();
//             var copy = factory.Create(gameContext);
//           
//             int entitiesCount = gameContext.count;
//             int entitiesCountCopy = copy.count;
//
//             if (entitiesCount != entitiesCountCopy)
//             {
//                 Assert.Fail($"{entitiesCount} {entitiesCountCopy}");
//             }
//
//             var originalEntities = gameContext.GetEntities();
//             var copyEntities = gameContext.GetEntities();
//
//             for (int i = 0; i < originalEntities.Length; i++)
//             {
//                 var originalEntity = originalEntities[i];
//                 var copyEntity = copyEntities[i];
//
//                 if (Math.Abs(copyEntity.damage.value - originalEntity.damage.value) > 0.001)
//                 {
//                     Assert.Fail();
//                 }
//             }
//         }
//     }
// }
