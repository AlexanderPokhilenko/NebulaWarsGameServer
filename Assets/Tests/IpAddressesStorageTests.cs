// using System;
// using System.Collections.Generic;
// using System.Net;
// using System.Threading;
// using NetworkLibrary.NetworkLibrary.Http;
// using NUnit.Framework;
// using Server.Udp.Storage;
//
// namespace Tests
// {
//     public class IpAddressesStorageTests
//     {
//         /// <summary>
//         /// В хранилище можно добавлять, читать, обновлять, удалять записи одновременно
//         /// </summary>
//         [Test]
//         public void Test1()
//         {
//             //Arrange
//             IpAddressesStorage ipAddressesStorage = new IpAddressesStorage();
//
//             object lockObj = new object();
//             List<int> lockMatchIds = new List<int>();
//             string exceptionMessage=null;
//             const int playersCount = 10;
//             const int matchesCount = 10000;
//             
//             //Поток на добавление
//             Thread threadAdd = new Thread(() =>
//             {
//                 try
//                 {
//                     int playerId = 1;
//                     for (int matchId = 1; matchId <= matchesCount; matchId++)
//                     {
//                         BattleRoyaleMatchModel model = new BattleRoyaleMatchModel()
//                         {
//                             MatchId = matchId,
//                             GameUnitsForMatch = new GameUnitsForMatch()
//                             {
//                                 Players = new List<PlayerInfoForMatch>()
//                             }
//                         };
//
//                         for (int i = 1; i <= playersCount; i++)
//                         {
//                             model.GameUnitsForMatch.Players.Add(new PlayerInfoForMatch
//                             {
//                                 TemporaryId = (ushort) playerId++
//                             });
//                         }
//
//                         ipAddressesStorage.AddMatch(model);
//                         lock (lockObj)
//                         {
//                             lockMatchIds.Add(matchId);
//                         }
//
//                         Console.WriteLine($"Матч добавлен "+matchId);
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     exceptionMessage = e.Message;
//                 }
//             });
//             threadAdd.Start();
//             
//             
//             //Act
//             //Поток на обновление
//             Thread threadUpdate = new Thread(() =>
//             {
//                 try
//                 {
//                     Random random = new Random();
//                     for (int i = 0; i < 10_000; i++)
//                     {
//                         if (lockMatchIds.Count != 0)
//                         {
//                             int randomNum = random.Next(1, matchesCount)%lockMatchIds.Count;
//                             int matchId;
//                             lock (lockObj)
//                             {
//                                 matchId = lockMatchIds[randomNum];
//                             }
//                     
//                             for (int playerId = 1; playerId <= playersCount; playerId++)
//                             {
//                                 ipAddressesStorage.TryUpdateIpEndPoint(matchId, playerId, new IPEndPoint(1, 1));
//                                 Console.WriteLine($"Ip обновлён {matchId} {playerId}");
//                             }
//                         }
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     exceptionMessage = e.Message;
//                 }
//             });
//             threadUpdate.Start();
//             
//                 
//             Random random1 = new Random();
//             for (int i = 0; i < 10_000; i++)
//             {
//                 if (lockMatchIds.Count != 0)
//                 {
//                     int randomNum = random1.Next(0, matchesCount)%lockMatchIds.Count;
//                     int matchId;
//                     lock (lockObj)
//                     {
//                         matchId = lockMatchIds[randomNum];
//                     }
//                 
//                     for (int playerId = 1; playerId <= playersCount; playerId++)
//                     {
//                         ipAddressesStorage.TryRemoveIpEndPoint(matchId, playerId);
//                         Console.WriteLine($"Ip удалён {matchId} {playerId}");
//                     }
//                 }
//                 
//             }
//
//             threadAdd.Join();
//             threadUpdate.Join();
//             
//             Assert.Null(exceptionMessage);
//         }
//     }
// }