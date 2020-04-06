﻿using System;
using System.Collections;
using UnityEngine;

namespace Server.GameEngine
{
    public class Clock
#if UNITY_5_3_OR_NEWER
    : MonoBehaviour
#endif
    {
        private static float prevTickTime;
        public static float deltaTime;
        private const float TickDeltaSeconds = 1f / 20;
        private Action action;
        
      

#if UNITY_5_3_OR_NEWER
      

        public void SetAction(Action actionArg)
        {
            if (action != null)
            {
                throw new Exception("Повторная инициализация таймера.");
            }
            else
            {
                action = actionArg;
            }
        }
        
        private IEnumerator MakeTick()
        {
            while (true)
            {
                var currentTime = Time.time;
                deltaTime = currentTime - prevTickTime;
                action.Invoke();
                prevTickTime = currentTime;
                yield return new WaitForSeconds(TickDeltaSeconds);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public void StartEndlessLoop()
        {
            prevTickTime = Time.time - TickDeltaSeconds;
            StartCoroutine(nameof(MakeTick));
        }

        //TODO почему это находится здесь?
        public void OnDestroy()
        {
            foreach (var battle in GameEngineMediator.MatchStorageFacade.GetAllGameSessions())
            {
                battle.FinishGame();
            }
        }
#else
        private static TimeSpan tickDeltaTime = new TimeSpan(0, 0, 0, 0, 30);
        private readonly GameEngineMediator gameEngineMediator;

        public Clock(GameEngineMediator gameEngineMediator)
        {
            this.gameEngineMediator = gameEngineMediator;
            deltaTime = (float)tickDeltaTime.TotalSeconds;
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

        private void WaitForNextTick(in DateTime nextTickStartTime)
        {
            var delay = nextTickStartTime - DateTime.UtcNow;
            deltaTime = (float)delay.TotalSeconds;
            if (delay.TotalMilliseconds > 0)
            {
                Thread.Sleep(delay);
            }
            // else
            // {
            //     throw new Exception("Сервер не успел отработать все системы за время между тиками");
            // }
        }
#endif
    }
}