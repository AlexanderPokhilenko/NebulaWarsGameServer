using System;
using Entitas;

namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Откатывает время на время ввода игроков и вычисляет коллизии
    /// </summary>
    public sealed class LagCompensationSystemGroup : IExecuteSystem
    {
        //Машина времени
        private readonly ITimeMachine timeMachine;

        //Набор систем лагкомпенсации
        private readonly LagCompensationSystem[] lagCompensationSystems;
     
        //Наша реализация кластеризатора
        private readonly TimeTravelMap travelMap = new TimeTravelMap();

        public LagCompensationSystemGroup(ITimeMachine timeMachine, 
            LagCompensationSystem[] lagCompensationSystems)
        {
            this.timeMachine = timeMachine;
            this.lagCompensationSystems = lagCompensationSystems;
        }
        
        public void Execute(GameState gs)
        {
            //На вход кластеризатор принимает текущее игровое состояние,
            //а на выход выдает набор «корзин». В каждой корзине лежат энтити,
            //которым для лагкомпенсации нужно одно и то же время из истории.
            var buckets = travelMap.RefillBuckets(gs);

            for (int bucketIndex = 0; bucketIndex < buckets.Count; bucketIndex++)
            {
                ProcessBucket(gs, buckets[bucketIndex]);
            }

            //В конце лагкомпенсации мы восстанавливаем физический мир 
            //в исходное состояние
            timeMachine.TravelToTime(gs.time);
        }

        private void ProcessBucket(GameState presentState, TimeTravelMap.Bucket bucket)
        {
            //Откатываем время один раз для каждой корзины
            GameState pastState = timeMachine.TravelToTime(bucket.Time);

            foreach (var lagCompensationSystem in lagCompensationSystems)
            {
                lagCompensationSystem.PastState = pastState;
                lagCompensationSystem.PresentState = presentState;

                foreach (var entity in bucket)
                {
                    lagCompensationSystem.Execute(entity);
                }
            }
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}