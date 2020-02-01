using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCannon", menuName = "Cannon", order = 54)]
public class CannonInfo : ScriptableObject
{
    public Vector2 position;
    public float cooldown;
    public BulletObject bulletObject;

    public void AddCannonToEntity(GameEntity entity)
    {
        entity.AddCannon(position, cooldown, bulletObject);
    }
}
