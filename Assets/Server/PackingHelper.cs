using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;

namespace Server
{
    public static class PackingHelper
    {
        public const int Mtu = 1500;
        public const int MaxSingleMessageSize = Mtu - (MessagesPack.IndexLength + 4);

        public static int GetByteLength(Dictionary<ushort, ViewTransform> dict) => 4 + dict.Count * (2 + 7);
        public static int GetByteLength(Dictionary<ushort, ushort> dict) => 4 + dict.Count * (2 + 2);

        public static Dictionary<TKey, TValue>[] Split<TKey, TValue>(this Dictionary<TKey, TValue> dict, int count)
        {
            var dictionaries = new Dictionary<TKey, TValue>[count];
            var capacity = (dict.Count - 1) / count + 1;

            for (var i = 0; i < count; i++)
            {
                dictionaries[i] = new Dictionary<TKey, TValue>(capacity);
            }

            var index = 0;
            foreach (var pair in dict)
            {
                dictionaries[index].Add(pair.Key, pair.Value);
                index++;
                index %= count;
            }

            return dictionaries;
        }
    }
}