using System;
using System.Collections;
using UnityEngine;

namespace Server.GameEngine
{
    /// <summary>
    /// Оно тикает.
    /// </summary>
    public class Chronometer: MonoBehaviour
    {
        private static float prevTickTime;
        public static float deltaTime;
        private const float TickDeltaSeconds = 1f / 30;
        private Action action;

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
        
        private IEnumerator MakeTicks()
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
            StartCoroutine(nameof(MakeTicks));
        }
    }
}