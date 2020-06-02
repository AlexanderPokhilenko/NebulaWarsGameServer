using Code.Common;
using Server;
using UnityEngine;

/// <summary>
/// Точка входа в проект.
/// </summary>
public class ServerController : MonoBehaviour
{
    private readonly ILog log = LogManager.CreateLogger(typeof(ServerController));
    private Startup startup;

    private void Awake()
    {
        LoggerConfig config = new LoggerConfig(100, 10_000, 
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
