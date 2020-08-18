using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DesperateDevs.Utils;

namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Разбивает игровое соостояие на наборы, которые нужно обработать в разных тиках
    /// </summary>
    public class TimeTravelMap
    {
        public class Bucket
        {
            public int TickNumber { get; }
            /// <summary>
            /// Сущности с снарядами
            /// </summary>
            public List<GameEntity> GameEntities = new List<GameEntity>();

            public Bucket(int tickNumber)
            {
                TickNumber = tickNumber;
            }
        }

        ///На вход кластеризатор принимает текущее игровое состояние,
        ///а на выход выдает набор «корзин». В каждой корзине лежат энтити,
        ///которым для лагкомпенсации нужно одно и то же время из истории.
        public List<Bucket> RefillBuckets(Contexts contexts)
        {
            Dictionary<int, Bucket> buckets = new Dictionary<int, Bucket>();
            var needTickEntities = contexts.game.GetGroup(GameMatcher.TickNumber)
                .GetEntities();

            foreach (var entity in needTickEntities)
            {
                int tick = entity.tickNumber.value;
                if (!buckets.ContainsKey(tick))
                {
                    buckets.Add(tick, new Bucket(tick));
                }
                
                buckets[tick].GameEntities.Add(entity);
            }
            
            return buckets.Values.ToList();
        }
    }
}