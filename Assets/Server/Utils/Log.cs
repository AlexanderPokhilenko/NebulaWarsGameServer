using UnityEngine;

namespace Server.Utils
{
    public static class Log
    {
        public static void Info(string message)
        {
            Debug.Log(message);
        }

        public static void Info(object obj)
        {
            Debug.Log(obj.ToString());
        }
        
        public static void Warning(string message)
        {
            Debug.LogWarning(message);
        }
        
        public static void Warning(object obj)
        {
            Debug.LogWarning(obj.ToString());
        }
        
        public static void Error(string message)
        {
            Debug.LogError(message);
        }
        
        public static void Error(object obj)
        {
            Debug.LogError(obj.ToString());
        }
    }
}