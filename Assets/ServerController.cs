using Server;
using UnityEngine;

/// <summary>
/// Точка входа в проект.
/// </summary>
public class ServerController : MonoBehaviour
{
    private Startup startup;

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
    }
}
