using System.Collections.Generic;

namespace Server.GameEngine
{
    public class PositionChunks
    {
        private readonly int diameter;
        private readonly int floatRadius;
        private readonly List<ushort>[,] chunks;
        private readonly Dictionary<(int x, int y), List<ushort>> additionalChunks;
        private readonly Stack<List<ushort>> additionalListsPool;
        public readonly Dictionary<ushort, ushort> SameChunkPairs;
        private const int PredictedSameTotalCapacity = 256;
        private const int PredictedNormalCapacity = 16;
        private const int PredictedAdditionalCapacity = 4;
        private const int PredictedAdditionalCount = 2;

        public PositionChunks(int mapRadius)
        {
            diameter = mapRadius * 2;
            floatRadius = mapRadius;
            chunks = new List<ushort>[diameter, diameter];
            for (var i = 0; i < diameter; i++)
            {
                for (var j = 0; j < diameter; j++)
                {
                    chunks[i, j] = new List<ushort>(PredictedNormalCapacity);
                }
            }

            SameChunkPairs = new Dictionary<ushort, ushort>(PredictedSameTotalCapacity);

            additionalChunks = new Dictionary<(int x, int y), List<ushort>>();

            additionalListsPool = new Stack<List<ushort>>(PredictedAdditionalCount);
            for (var i = 0; i < PredictedAdditionalCount; i++)
            {
                additionalListsPool.Push(new List<ushort>(PredictedAdditionalCapacity));
            }
        }

        public void Fill(List<GameEntity> entities)
        {
            Clear();

            var entitiesCount = entities.Count;
            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = entities[i];
                var id = entity.id.value;
                var position = entity.globalTransform.position;
                var colliderRadius = entity.circleCollider.radius;

                var floatX = position.x + floatRadius;
                var floatY = position.y + floatRadius;

                var minX = (int)(floatX - colliderRadius);
                var maxX = (int)(floatX + colliderRadius);
                var minY = (int)(floatY - colliderRadius);
                var maxY = (int)(floatY + colliderRadius);

                for (var x = minX; x <= maxX; x++)
                {
                    for (var y = minY; y <= maxY; y++)
                    {
                        List<ushort> list;
                        if (x >= 0 && y >= 0 && x < diameter && y < diameter)
                        {
                            list = chunks[x, y];
                        }
                        else
                        {
                            var key = (x, y);
                            if (!additionalChunks.TryGetValue(key, out list))
                            {
                                if (additionalListsPool.Count > 0)
                                {
                                    list = additionalListsPool.Pop();
                                }
                                else
                                {
                                    list = new List<ushort>(PredictedAdditionalCapacity);
                                }
                                additionalChunks.Add(key, list);
                            }
                        }

                        var prevItemsCount = list.Count;
                        list.Add(id);

                        for (var j = 0; j < prevItemsCount; j++)
                        {
                            var prevId = list[j];

                            if (prevId > id)
                            {
                                SameChunkPairs[id] = prevId;
                            }
                            else
                            {
                                SameChunkPairs[prevId] = id;
                            }
                        }
                    }
                }
            }
        }

        private void Clear()
        {
            SameChunkPairs.Clear();

            for (var i = 0; i < diameter; i++)
            {
                for (var j = 0; j < diameter; j++)
                {
                    chunks[i, j].Clear();
                }
            }

            if (additionalChunks.Count > 0)
            {
                foreach (var additionalList in additionalChunks.Values)
                {
                    additionalList.Clear();
                    additionalListsPool.Push(additionalList);
                }
                additionalChunks.Clear();
            }
        }
    }
}