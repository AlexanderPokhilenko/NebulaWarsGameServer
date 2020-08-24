// using Server.GameEngine;
// using System.Collections.Generic;
// using Server.GameEngine.Chronometers;
//
// public class AlternationShooter : SpecialShooter
// {
//     private readonly float timeDelay;
//     private readonly int groupCount;
//     private readonly int twinGroupCount;
//     private int framesCount = int.MaxValue;
//     private bool odd;
//
//     public AlternationShooter(float timeDelay, int groupCount)
//     {
//         this.timeDelay = timeDelay;
//         this.groupCount = groupCount;
//         twinGroupCount = 2 * groupCount;
//     }
//
//     protected override IEnumerable<ServerGameEntity> GetSpecialCannons(List<ServerGameEntity> cannons)
//     {
//         if (framesCount >= timeDelay / Chronometer.DeltaTime)
//         {
//             odd = !odd;
//             framesCount = 0;
//             var i = 0;
//             if (cannons.Count % 2 != 0)
//             {
//                 i++;
//                 yield return cannons[0];
//             }
//
//             if (odd) i += groupCount;
//
//             for (; i < cannons.Count; i+= twinGroupCount)
//             {
//                 for (var j = i; j < i + groupCount; j++)
//                 {
//                     yield return cannons[j];
//                 }
//             }
//         }
//
//         framesCount++;
//     }
// }