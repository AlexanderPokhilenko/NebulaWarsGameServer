// using System.Linq;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "NewRandomObject", menuName = "BaseObjects/RandomObject", order = 50)]
// public class RandomObject : EntityCreatorObject
// {
//     private System.Random random;
//     private int maxProbability;
//     public ProbabilityObject[] objects;
//
//     void OnEnable()
//     {
//         random = new System.Random();
//         maxProbability = objects.Sum(o => o.probability);
//     }
//
//     public override void FillEntity(GameContext context, GameEntity entity)
//     {
//         var probability = random.Next(maxProbability);
//
//         EntityCreatorObject result = null;
//
//         foreach (var probabilityObject in objects)
//         {
//             if (probabilityObject.TryGetObject(ref probability, out result)) break;
//         }
//
//         if (result == null)
//         {
//             entity.isDestroyed = true;
//         }
//         else
//         {
//             result.FillEntity(context, entity);
//         }
//     }
// }
