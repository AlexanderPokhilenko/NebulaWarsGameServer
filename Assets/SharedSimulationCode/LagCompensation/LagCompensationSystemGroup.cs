using System.Collections.Generic;
using Entitas;

namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Откатывает время на время ввода игроков и вычисляет коллизии
    /// </summary>
    public class LagCompensationSystemGroup:IExecuteSystem
    {
        private readonly Contexts contexts;
        private readonly ITimeMachine timeMachine;
        private readonly IGameStateHistory gameStateHistory;
        private readonly TimeTravelMap travelMap = new TimeTravelMap();
        private readonly LagCompensationSystem[] lagCompensationSystems;

        public LagCompensationSystemGroup(Contexts contexts, ITimeMachine timeMachine,
            LagCompensationSystem[] lagCompensationSystems, IGameStateHistory gameStateHistory)
        {
            this.contexts = contexts;
            this.timeMachine = timeMachine;
            this.lagCompensationSystems = lagCompensationSystems;
            this.gameStateHistory = gameStateHistory;
        }
        
        public void Execute()
        {
            timeMachine.SetActualGameState(gameStateHistory.GetActualGameState());
            
            
            List<TimeTravelMap.Bucket> buckets = travelMap.RefillBuckets(contexts);
            for (int bucketIndex = 0; bucketIndex < buckets.Count; bucketIndex++)
            {
                TimeTravelMap.Bucket bucket = buckets[bucketIndex];
                ProcessBucket(bucket);
            }

            //В конце лагкомпенсации мы восстанавливаем физический мир 
            //в исходное состояние
            int tickNumber = gameStateHistory.GetLastTickNumber();
            timeMachine.TravelToTime(tickNumber);
        }

        private void ProcessBucket(
            // GameState presentState,
            TimeTravelMap.Bucket bucket)
        {
            //Откатываем время один раз для каждой корзины
            // GameState pastState =
            
            //Сдвигает 3d модели к определённому моменту времени в прошлое
            timeMachine.TravelToTime(bucket.TickNumber);

            foreach (var lagCompensationSystem in lagCompensationSystems)
            {
                //todo зачем это нужно?
                // lagCompensationSystem.PastState = pastState;
                // lagCompensationSystem.PresentState = presentState;

                foreach (GameEntity entity in bucket.GameEntities)
                {
                    lagCompensationSystem.Execute(entity);
                }
            }
        }
    }
}