using System;
using System.Collections;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using UnityEngine;

namespace Server.GameEngine.Chronometers
{
    /// <summary>
    /// Оно тикает.
    /// </summary>
    public class Chronometer: MonoBehaviour
    {
        private Action callback;
        private const float MaxDelay = ServerTimeConstants.MinDeltaTime;

        /// <summary>
        /// Время в секундах, которое потребовалось для обработки последнего тика.
        /// </summary>
        /// <remarks>
        /// Это НЕ постоянная величина и она может изменяться из-за нагрузки на сервер.
        /// </remarks>
        public static float DeltaTime { get; private set; } = ServerTimeConstants.MinDeltaTime;
        
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
            //Время "виртуального" предыдущего кадра
            var prevTickStartTime = Time.time - MaxDelay;

            while (true)
            {
                var tickStartTime = Time.time;
                
                //Время между началами кадров.
                DeltaTime = tickStartTime - prevTickStartTime;
                prevTickStartTime = tickStartTime;

                callback.Invoke();

                var tickEndTime = Time.time;

                // Время обработки кадра
                var tickDelta = tickEndTime - tickStartTime;

                var sleepDelay = MaxDelay - tickDelta;

                if (sleepDelay > 0f)
                {
                    yield return new WaitForSeconds(sleepDelay);
                }
                else
                {
                    Debug.LogWarning(nameof(Chronometer) + ": Опаздываем, сервер не успел отработать тик за положенное время.");
                    yield return null;
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