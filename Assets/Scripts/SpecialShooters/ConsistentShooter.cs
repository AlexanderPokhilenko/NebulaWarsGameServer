// using System.Collections.Generic;
// using Server.GameEngine;
// using Server.GameEngine.Chronometers;
//
// public class ConsistentShooter : SpecialShooter
// {
//     private readonly float timeDelay;
//     private readonly int cannonsInGroup;
//     private readonly int groupsCount;
//     private readonly bool mirroring;
//     private readonly int cannonsIncrement;
//     private int framesCount = int.MaxValue;
//     private int currentStep;
//
//     public ConsistentShooter(float timeDelay, int cannonsInGroup, int groupsCount, bool mirroring)
//     {
//         this.timeDelay = timeDelay;
//         this.cannonsInGroup = cannonsInGroup;
//         this.groupsCount = groupsCount;
//         this.mirroring = mirroring;
//         cannonsIncrement = groupsCount * cannonsInGroup;
//     }
//
//     protected override IEnumerable<GameEntity> GetSpecialCannons(List<GameEntity> cannons)
//     {
//         if (framesCount >= timeDelay / Chronometer.DeltaTime)
//         {
//             framesCount = 0;
//             var cannonsCount = cannons.Count;
//             var fullGroupSetsCount = cannonsCount / cannonsIncrement; // Количество полных наборов групп
//             var i = 0;
//             int extraCannons;
//             if (mirroring)
//             {
//                 extraCannons = cannonsCount - ((fullGroupSetsCount / 2) * cannonsIncrement * 2); // У нас должно быть чётное количество полных наборов групп
//             }
//             else
//             {
//                 extraCannons = cannonsCount % cannonsInGroup; // У нас должны быть полные группы
//             }
//             var odd = true;
//             while (i < extraCannons)
//             {
//                 if (mirroring)
//                 {
//                     if (odd)
//                     {
//                         yield return cannons[i++ / 2];
//                     }
//                     else
//                     {
//                         yield return cannons[cannonsCount - 1 - i++ / 2];
//                     }
//                     odd = !odd;
//                 }
//                 else
//                 {
//                     yield return cannons[i++];
//                 }
//             }
//
//             if (mirroring && i > 0)
//             {
//                 i = (i - 1) / 2 + 1; // Деление на 2 с округлением вверх - пропускаем обработанные пушки в начале
//                 cannonsCount -= extraCannons - i; // Убираем уже обработанные пушки в конце
//             }
//
//
//             if (mirroring)
//             {
//                 var middleIndex = cannonsIncrement * fullGroupSetsCount / 2 + i;
//
//                 for (i += cannonsInGroup * currentStep; i < middleIndex; i += cannonsIncrement)
//                 {
//                     for (var j = i; j < i + cannonsInGroup; j++)
//                     {
//                         yield return cannons[j];
//                     }
//                 }
//
//                 for (i = cannonsCount - cannonsInGroup * currentStep - 1; i >= middleIndex; i -= cannonsIncrement)
//                 {
//                     for (var j = i; j > i - cannonsInGroup; j--)
//                     {
//                         yield return cannons[j];
//                     }
//                 }
//
//             }
//             else
//             {
//                 for (i += cannonsInGroup * currentStep; i < cannonsCount; i += cannonsIncrement)
//                 {
//                     for (var j = i; j < i + cannonsInGroup; j++)
//                     {
//                         yield return cannons[j];
//                     }
//                 }
//             }
//
//             currentStep++;
//             if (currentStep >= groupsCount) currentStep = 0;
//         }
//
//         framesCount++;
//     }
// }