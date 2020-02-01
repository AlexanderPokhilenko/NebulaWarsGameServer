using System;
using System.Threading;
using AmoebaBattleServer01.Experimental.GameEngine.Systems;

namespace AmoebaBattleServer01.Experimental.GameEngine
{
    public class Clock
    {
        private static TimeSpan tickDeltaTime = new TimeSpan(0, 0, 0, 0, 30);
        private readonly GameEngineMediator gameEngineMediator;

        public Clock(GameEngineMediator gameEngineMediator)
        {
            this.gameEngineMediator = gameEngineMediator;
        }

        public void StartEndlessLoop()
        {
            while (true)
            {
                DateTime nextTickStartTime = GetNextTickTime();
                Tick();
                WaitForNextTick(nextTickStartTime);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private DateTime GetNextTickTime()
        {
            var tickStartTime = DateTime.UtcNow;
            DateTime nextTickStartTime = tickStartTime.AddMilliseconds(tickDeltaTime.TotalMilliseconds);
            return nextTickStartTime;
        }
        private void Tick()
        {
            gameEngineMediator.Tick();
        }

        private void WaitForNextTick(in DateTime nextTickStartTime)
        {
            var delay = nextTickStartTime - DateTime.UtcNow;
            if (delay.TotalMilliseconds > 0)
            {
                Thread.Sleep(delay);
            }
            // else
            // {
            //     throw new Exception("Сервер не успел отработать все системы за время между тиками");
            // }
        }
    }
}