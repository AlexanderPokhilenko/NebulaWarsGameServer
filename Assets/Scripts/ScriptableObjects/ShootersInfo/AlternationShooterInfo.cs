using UnityEngine;

[CreateAssetMenu(fileName = "NewAlternationShooter", menuName = "SpecialShooters/AlternationShooter", order = 51)]
public class AlternationShooterInfo : ShooterInfo
{
    [Min(0)]
    public float timeDelay;
    [Min(1)]
    public int cannonsInGroup = 1;

    public override SpecialShooter CreateInstance() => new AlternationShooter(timeDelay, cannonsInGroup);
}