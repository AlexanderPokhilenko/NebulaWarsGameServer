using System;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;

namespace Server
{
    public static class PackingHelper
    {
        public const int Mtu = 1200;
        public const int MaxSingleMessageSize = Mtu - (MessagesPack.IndexLength + 4 + 30);

        public static int GetByteLength(Dictionary<ushort, ViewTransform> dict) => 4 + dict.Count * (2 + 7);
        public static int GetByteLength(Dictionary<ushort, ushort> dict) => 4 + dict.Count * (2 + 2);
        public static int GetByteLength(ushort[] arr) => 4 + arr.Length * 2;

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

        public static T[][] Split<T>(this T[] arr, int count)
        {
            var arrays = new T[count][];
            var maxCapacity = (arr.Length - 1) / count + 1;
            var lastArrayIndex = count - 1;

            for (var i = 0; i < lastArrayIndex; i++)
            {
                arrays[i] = new T[maxCapacity];
            }

            // Последний массив может содержать меньше элементов
            var lastCapacity = arr.Length - maxCapacity * lastArrayIndex;
            arrays[lastArrayIndex] = new T[lastCapacity];

            var index = 0;
            for (var i = 0; i < lastArrayIndex; i++)
            {
                Array.Copy(arr, index, arrays[i], 0, maxCapacity);
                index += maxCapacity;
            }

            Array.Copy(arr, index, arrays[lastArrayIndex], 0, lastCapacity);

            return arrays;
        }
    }
}