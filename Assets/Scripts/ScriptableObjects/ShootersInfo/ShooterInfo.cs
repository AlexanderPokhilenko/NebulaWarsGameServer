using UnityEngine;

public abstract class ShooterInfo : ScriptableObject
{
    public abstract SpecialShooter CreateInstance();
}
