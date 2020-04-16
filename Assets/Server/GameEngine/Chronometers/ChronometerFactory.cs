using System;
using UnityEngine;

namespace Server.GameEngine
{
    /// <summary>
    /// Создаёт game object на сцене и цепляет на него хронометр.
    /// </summary>
    public static class ChronometerFactory
    {
        public static Chronometer Create(Action action)
        {
            var go = new GameObject("Chronometer");
            var clock = go.AddComponent<Chronometer>();
            clock.SetCallback(action);
            return clock;
        }
    }
}