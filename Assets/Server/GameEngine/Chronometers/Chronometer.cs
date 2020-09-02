using System;
using System.Collections;
using System.Threading.Tasks;
using Plugins.submodules.SharedCode;
using Plugins.submodules.SharedCode.Logger;
using Plugins.submodules.SharedCode.Physics;
using UnityEngine;

namespace Server.GameEngine.Chronometers
{
    /// <summary>
    /// Оно тикает.
    /// </summary>
    public class Chronometer: MonoBehaviour, ITickDeltaTimeStorage, ITickStartTimeStorage
    {
        private Action callback;
        private DateTime prevTickStartTime;
        [SerializeField] private float tickStartDeltaTimeSec;
        [SerializeField] private string sleepDelaySec;
        [SerializeField] private string tickExecutionTimeSec;
        private readonly ILog log = LogManager.CreateLogger(typeof(Chronometer));
        private readonly TimeSpan maxTickStartDelay = TimeSpan.FromSeconds(1f/Globals.TargetTickRatePerSecond);
        

        public void SetCallback(Action callbackArg)
        {
            callback = callbackArg;
        }
        
        public void StartEndlessLoop()
        {
            StartCoroutine(MakeTicks());
        }
        
        public float GetDeltaTimeSec()
        {
            return tickStartDeltaTimeSec;
        }
        
        public DateTime GetTickStartTime()
        {
            return prevTickStartTime;
        }
        
        private IEnumerator MakeTicks()
        {
            prevTickStartTime = DateTime.UtcNow - maxTickStartDelay;
            while (true)
            {
                DateTime currentTickStartTime = DateTime.UtcNow;
                tickStartDeltaTimeSec = (float) (currentTickStartTime - prevTickStartTime).TotalSeconds;
                
                try
                {
                    //Обработка игровой логики
                    callback.Invoke();
                }
                catch (Exception e)
                {
                    log.Error(e.FullMessage());
                }
    
                TimeSpan tickExecutionTime = DateTime.UtcNow - currentTickStartTime;
                if (maxTickStartDelay.TotalSeconds + 0.020f < tickExecutionTime.TotalSeconds)
                {
                    log.Error("Слишком долгая обработка " +
                              $"tickExecutionTime.TotalSeconds = {tickExecutionTime.TotalSeconds} " +
                              $"maxTickStartDelay.TotalSeconds = {maxTickStartDelay.TotalSeconds}");
                }

                //Выполнение Update других скриптов
                yield return null;
                
                //Ожидание если есть запас времени
                TimeSpan tickAndUnityUpdateExecutionTime = (DateTime.UtcNow - currentTickStartTime);
                TimeSpan sleepDelay = maxTickStartDelay - tickAndUnityUpdateExecutionTime;
                sleepDelaySec =$"{sleepDelay.TotalSeconds:0.000}";
                tickExecutionTimeSec =$"{tickExecutionTime.TotalSeconds:0.000}";
                if (sleepDelay.TotalSeconds > 0)
                {
                    Task.Delay(sleepDelay).Wait();
                }
                else
                {
                    log.Error($"Опаздываем. Логика = {tickExecutionTime.TotalSeconds} " +
                              $"Логика + update = {tickAndUnityUpdateExecutionTime.TotalSeconds}");
                }
                
                prevTickStartTime = currentTickStartTime;
            }
        }

       
    }
}