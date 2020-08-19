// using UnityEngine;
//
// [CreateAssetMenu(fileName = "NewConsistentShooter", menuName = "SpecialShooters/ConsistentShooter", order = 52)]
// public class ConsistentShooterInfo : ShooterInfo
// {
//     [Min(0)]
//     public float timeDelay;
//     [Min(1)]
//     public int cannonsInGroup = 1;
//     [Min(2)]
//     public int groupsCount = 3;
//
//     public bool mirroring;
//
//     public override SpecialShooter CreateInstance() => new ConsistentShooter(timeDelay, cannonsInGroup, groupsCount, mirroring);
// }