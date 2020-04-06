using System;
using UnityEngine;

namespace Server.GameEngine
{
    /// <summary>
    /// Вызывает метод раз в период.
    /// </summary>
    public class ClockFacade
    {
        private readonly Clock clock;

        public ClockFacade(Action action)
        {
#if UNITY_5_3_OR_NEWER
            var go = new GameObject("Clock");
            clock = go.AddComponent<Clock>();
            clock.SetAction(action);
#else
            clock = new Clock(this);
#endif
        }
   
        public void StartEndlessLoop()
        {
            clock.StartEndlessLoop();
        }
    }
}