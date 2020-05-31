using System;
using Entitas;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Убивает всех ботов после задержки
    /// </summary>
    public class TestEndMatchSystem : IExecuteSystem
    {
        private readonly Clockwork clockwork;
        private readonly IGroup<GameEntity> botsGroup;
        private readonly GameContext gameContext;

        public TestEndMatchSystem(Contexts contexts)
        {
            gameContext = contexts.game;
            botsGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Bot,
                GameMatcher.HealthPoints));
            TimeSpan delayToKillBots = new TimeSpan(0, 0, 0, 2);
            clockwork = new Clockwork(delayToKillBots);
        }
        
        public void Execute()
        {
            if (clockwork.IsOk())
            {
                KillAllBots();
            }
        }

        private void KillAllBots()
        {
            GameEntity zone = gameContext.zone.GetZone(gameContext);
            zone.ReplaceCircleCollider(5f);
            foreach (var bot in botsGroup)
            {
                bot.ReplaceHealthPoints(1);
            }
        }

        private class Clockwork
        {
            private readonly DateTime okTime;
        
            public Clockwork(TimeSpan delay)
            {
                okTime = DateTime.Now+delay;
            }

            public bool IsOk()
            {
                return DateTime.Now > okTime;
            }
        }
    }
}