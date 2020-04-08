using System;
using UnityEngine;

namespace Server.GameEngine
{
    /// <summary>
    /// Создаёт game object на сцене и цепляет на него таймер.
    /// </summary>
    public static class ChronometerFactory
    {
        public static Chronometer Create(Action action)
        {
            var go = new GameObject("Chronometer");
            var clock = go.AddComponent<Chronometer>();
            clock.SetAction(action);
            return clock;
        }
    }
}