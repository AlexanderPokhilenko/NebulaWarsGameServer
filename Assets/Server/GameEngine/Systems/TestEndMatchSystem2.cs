using System;
using Entitas;

namespace Server.GameEngine.Systems
{
    public class TestEndMatchSystem2 : IExecuteSystem
    {
        readonly Clockwork clockwork;
        readonly IGroup<GameEntity> botsGroup;
        
        public TestEndMatchSystem2(Contexts contexts)
        {
            botsGroup = contexts.game.GetGroup(GameMatcher.AnyOf(GameMatcher.Bot, GameMatcher.HealthPoints));
            clockwork = new Clockwork(new TimeSpan(0,0,0,2));
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
            var bots = botsGroup.GetEntities();
            foreach (var bot in bots)
            {
                bot.ReplaceHealthPoints(0);
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