#define EnableConsoleLogs

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
#if EnableConsoleLogs
        if (loggingEvent.Level.CompareTo(Level.Error) >= 0)
        {
            Debug.LogError(RenderLoggingEvent(loggingEvent));
        }
        else if (loggingEvent.Level.CompareTo(Level.Warn) >= 0)
        {
            Debug.LogWarning(RenderLoggingEvent(loggingEvent));
        }
        else
        {
            Debug.Log(RenderLoggingEvent(loggingEvent));
        }
#endif    
    }
}