// #define EnableInfoLogLevel
// #define EnableConsoleLogs

using log4net.Appender;
using log4net.Core;
using UnityEngine;

/// <summary>
/// Не переименовывать. Не менять пространство имён. Иначе нужно изменить конфиг файл логгера (log4net.xml).
/// </summary>
// ReSharper disable once CheckNamespace
// ReSharper disable once UnusedType.Global
public class UnityDebugAppender : AppenderSkeleton
{
    protected override void Append(LoggingEvent loggingEvent)
    {
        if (loggingEvent.Level.CompareTo(Level.Error) >= 0)
        {
            #if EnableConsoleLogs
                Debug.LogError(RenderLoggingEvent(loggingEvent));
            #endif
        }
        else if (loggingEvent.Level.CompareTo(Level.Warn) >= 0)
        {
            #if EnableConsoleLogs
                Debug.LogWarning(RenderLoggingEvent(loggingEvent));
            #endif
        }
        #if EnableInfoLogLevel
        else if (loggingEvent.Level.CompareTo(Level.Notice) >= 0)
        {
            #if EnableConsoleLogs
                Debug.Log(RenderLoggingEvent(loggingEvent));
            #endif
        }
        #endif
    }
}