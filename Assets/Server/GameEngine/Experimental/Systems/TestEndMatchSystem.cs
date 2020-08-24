// using System;
// using Entitas;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     /// <summary>
//     /// Убивает всех ботов после задержки
//     /// </summary>
//     public class TestEndMatchSystem : IExecuteSystem
//     {
//         private readonly Clockwork clockwork;
//         private readonly IGroup<ServerGameEntity> botsGroup;
//         private readonly ServerGameContext gameContext;
//
//         public TestEndMatchSystem(Contexts contexts)
//         {
//             gameContext = contexts.serverGame;
//             botsGroup = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Bot,
//                 ServerGameMatcher.HealthPoints));
//             TimeSpan delayToKillBots = new TimeSpan(0, 0, 0, 2);
//             clockwork = new Clockwork(delayToKillBots);
//         }
//         
//         public void Execute()
//         {
//             if (clockwork.IsOk())
//             {
//                 KillAllBots();
//             }
//         }
//
//         private void KillAllBots()
//         {
//             ServerGameEntity zone = gameContext.zone.GetZone(gameContext);
//             zone.ReplaceCircleCollider(5f);
//             foreach (var bot in botsGroup)
//             {
//                 bot.ReplaceHealthPoints(1);
//             }
//         }
//
//         private class Clockwork
//         {
//             private readonly DateTime okTime;
//         
//             public Clockwork(TimeSpan delay)
//             {
//                 okTime = DateTime.Now+delay;
//             }
//
//             public bool IsOk()
//             {
//                 return DateTime.Now > okTime;
//             }
//         }
//     }
// }