using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTorpedo", menuName = "BaseObjects/Torpedo", order = 60)]
public class TorpedoObject : BulletObject
{
    public float distance;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddChaser(distance);

        return entity;
    }
}
