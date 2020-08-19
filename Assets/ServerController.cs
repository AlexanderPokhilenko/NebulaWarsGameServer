using Plugins.submodules.SharedCode;
using Plugins.submodules.SharedCode.Logger;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using Server;
using UnityEngine;

/// <summary>
/// Точка входа в проект.
/// </summary>
public class ServerController : MonoBehaviour
{
    private Startup startup;
    private readonly ILog log = LogManager.CreateLogger(typeof(ServerController));

    private void Awake()
    {
        Application.targetFrameRate = (int) ServerTimeConstants.MaxFps;
        UnityThread.InitUnityThread();
        LoggerConfig config = new LoggerConfig(100, 1000, 
            Application.persistentDataPath);
        if (LogManager.TrySetConfig(config))
        {
            log.Info("Путь к файлу с логами "+config.PersistentDataPath);    
        }
    }

    private void Start()
    {
        startup = new Startup();
        startup.Run();
    }
    
    private void OnDestroy()
    {
        //Уведомление матчмейкера о завершении матчей
        startup.FinishAllMatches();
        
        //Это нужно для того, чтобы после остановки unity проекта потоки остановились и не писали в консоль.
        startup.StopAllThreads();
        
        LogManager.Print();
    }
}
