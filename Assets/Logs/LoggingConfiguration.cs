using System;
using System.IO;
using log4net.Config;
using UnityEngine;


/// <summary>
/// Этот класс жизненно необходим для работы логгера.
/// </summary>
// ReSharper disable once CheckNamespace
public static class LoggingConfiguration
{
    const string ConfigFileName = "log4net";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Configure()
    {
        byte[] xml = (Resources.Load(ConfigFileName , typeof(TextAsset)) as TextAsset)?.bytes;
        if(xml == null) throw new Exception("Can not read xml configuration for logger.");
        XmlConfigurator.Configure(new MemoryStream(xml));
    }
}
    