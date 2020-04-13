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
        private Action callback;
        private const float MaxDelay = 1f/30;

        //TODO говно
        public static float GetMagicDich()
        {
            return 1f/20;
        }
        
        public void SetCallback(Action actionArg)
        {
            if (callback != null)
            {
                throw new Exception("Повторная инициализация таймера.");
            }
            else
            {
                callback = actionArg;
            }
        }
        
        private IEnumerator MakeTicks()
        {
            while (true)
            {
                float executionStartTime = Time.time;
                callback.Invoke();
                float sleepDelay = MaxDelay - (Time.time - executionStartTime);
                if (sleepDelay > 0)
                {
                    yield return new WaitForSeconds(sleepDelay);
                }
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public void StartEndlessLoop()
        {
            StartCoroutine(nameof(MakeTicks));
        }
    }
}