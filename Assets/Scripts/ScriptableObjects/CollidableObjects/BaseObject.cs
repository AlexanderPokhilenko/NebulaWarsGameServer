using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseObject", menuName = "BaseObjects/BaseObject", order = 51)]
public class BaseObject : EntityCreatorObject
{
    public ViewTypeId typeId;
    public ColliderInfo colliderInfo;
    [Min(0)]
    public float mass;
    public CannonInfo cannonInfo;
    public bool isUnmovable;
    public bool isPassingThrough;
    [Min(0)]
    public float collisionDamage;
    public List<EntityCreatorObject> dropObjects;
    public PartObject[] parts;
    public ShooterInfo shooter;

    public override void FillEntity(GameContext context, GameEntity entity)
    {
        if(typeId != ViewTypeId.Invisible) entity.AddViewType(typeId);
        if (colliderInfo != null)
        {
            entity.isCollidable = true;
            colliderInfo.AddColliderToEntity(entity);
        }

        if(mass > 0) entity.AddMass(mass);

        if(cannonInfo != null) cannonInfo.AddCannonToEntity(entity);

        entity.isUnmovable = isUnmovable;
        entity.isPassingThrough = isPassingThrough;
        if(collisionDamage > 0) entity.AddDamage(collisionDamage);

        if(dropObjects != null && dropObjects.Any()) entity.AddDrop(new List<EntityCreatorObject>(dropObjects));

        if (parts != null)
        {
            foreach (var part in parts)
            {
                part.AddPartToEntity(context, entity);
            }
        }

        if(shooter != null) entity.AddSpecialShooter(shooter.CreateInstance());
    }
}
