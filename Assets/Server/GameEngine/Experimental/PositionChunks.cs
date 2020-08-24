// using System.Collections.Generic;
//
// namespace Server.GameEngine.Experimental
// {
//     public class PositionChunks
//     {
//         private readonly int diameter;
//         private readonly int floatRadius;
//         private readonly List<ushort>[,] chunks;
//         private readonly Dictionary<(int x, int y), List<ushort>> additionalChunks;
//         private readonly Stack<List<ushort>> additionalListsPool;
//         public readonly HashSet<(ushort CurrentId, ushort OtherId)> SameChunkPairs;
//         private const int PredictedNormalCapacity = 16;
//         private const int PredictedAdditionalCapacity = 4;
//         private const int PredictedAdditionalCount = 2;
//         private const float MicroIncrement = 0.001f;
//
//         public PositionChunks(int mapRadius)
//         {
//             diameter = mapRadius * 2;
//             floatRadius = mapRadius;
//             chunks = new List<ushort>[diameter, diameter];
//             for (var i = 0; i < diameter; i++)
//             {
//                 for (var j = 0; j < diameter; j++)
//                 {
//                     chunks[i, j] = new List<ushort>(PredictedNormalCapacity);
//                 }
//             }
//
//             SameChunkPairs = new HashSet<(ushort CurrentId, ushort OtherId)>();
//
//             additionalChunks = new Dictionary<(int x, int y), List<ushort>>();
//
//             additionalListsPool = new Stack<List<ushort>>(PredictedAdditionalCount);
//             for (var i = 0; i < PredictedAdditionalCount; i++)
//             {
//                 additionalListsPool.Push(new List<ushort>(PredictedAdditionalCapacity));
//             }
//         }
//
//         public void Fill(List<ServerGameEntity> entities, ServerGameContext context)
//         {
//             Clear();
//
//             var entitiesCount = entities.Count;
//             for (var i = 0; i < entitiesCount; i++)
//             {
//                 var entity = entities[i];
//                 var id = entity.id.value;
//                 var position = entity.hasGlobalTransform ? entity.globalTransform.position : entity.GetGlobalPositionVector2(context);
//                 var colliderRadius = entity.circleCollider.radius + MicroIncrement;
//
//                 var floatX = position.x + floatRadius;
//                 var floatY = position.y + floatRadius;
//
//                 var minX = (int)(floatX - colliderRadius);
//                 var maxX = (int)(floatX + colliderRadius);
//                 var minY = (int)(floatY - colliderRadius);
//                 var maxY = (int)(floatY + colliderRadius);
//
//                 for (var x = minX; x <= maxX; x++)
//                 {
//                     for (var y = minY; y <= maxY; y++)
//                     {
//                         List<ushort> list;
//                         if (x >= 0 && y >= 0 && x < diameter && y < diameter)
//                         {
//                             list = chunks[x, y];
//                         }
//                         else
//                         {
//                             var key = (x, y);
//                             if (!additionalChunks.TryGetValue(key, out list))
//                             {
//                                 if (additionalListsPool.Count > 0)
//                                 {
//                                     list = additionalListsPool.Pop();
//                                 }
//                                 else
//                                 {
//                                     list = new List<ushort>(PredictedAdditionalCapacity);
//                                 }
//                                 additionalChunks.Add(key, list);
//                             }
//                         }
//
//                         var prevItemsCount = list.Count;
//                         list.Add(id);
//
//                         for (var j = 0; j < prevItemsCount; j++)
//                         {
//                             var prevId = list[j];
//
//                             if (prevId > id)
//                             {
//                                 SameChunkPairs.Add((id, prevId));
//                             }
//                             else
//                             {
//                                 SameChunkPairs.Add((prevId, id));
//                             }
//                         }
//                     }
//                 }
//             }
//
//             //string str = "";
//             //for (int y = diameter - 1; y >= 0; y--)
//             //{
//             //    for (int x = 0; x < diameter; x++)
//             //    {
//             //        str += "[" + string.Join(", ", chunks[x, y]) + "]";
//             //    }
//
//             //    str += "\n";
//             //}
//             //Debug.Log(str);
//             //Debug.Log(string.Join("; ", SameChunkPairs));
//             //Debug.Log(SameChunkPairs.Count);
//         }
//
//         private void Clear()
//         {
//             SameChunkPairs.Clear();
//
//             for (var i = 0; i < diameter; i++)
//             {
//                 for (var j = 0; j < diameter; j++)
//                 {
//                     chunks[i, j].Clear();
//                 }
//             }
//
//             if (additionalChunks.Count > 0)
//             {
//                 foreach (var additionalList in additionalChunks.Values)
//                 {
//                     additionalList.Clear();
//                     additionalListsPool.Push(additionalList);
//                 }
//                 additionalChunks.Clear();
//             }
//         }
//     }
// }