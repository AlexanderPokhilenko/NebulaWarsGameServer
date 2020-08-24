using UnityEngine;

public abstract class AbilityInfo : ScriptableObject
{
    public abstract void AddAbilityToEntity(ServerGameEntity entity);
}
