// using UnityEngine;
//
// [CreateAssetMenu(fileName = "NewBonusAdder", menuName = "BaseObjects/Bonuses/BonusAdder", order = 51)]
// public class BonusAdderObject : BaseObject
// {
//     public BaseObject bonusObject;
//     [Min(0)]
//     public float duration;
//     public bool colliderInheritance = true;
//
//     public override void FillEntity(ServerGameContext context, ServerGameEntity entity)
//     {
//         base.FillEntity(context, entity);
//         entity.AddBonusAdder(bonusObject, duration, colliderInheritance);
//     }
// }
