using Server;
using Server.GameEngine;
using UnityEngine;

/// <summary>
/// Точка входа в проект.
/// </summary>
public class ServerController : MonoBehaviour
{
    private GameServer gameServer;
    
    private void Start()
    {
        gameServer = new GameServer();
        gameServer.Run();
    }
    
    private void OnDestroy()
    {
        foreach (var match in GameEngineTicker.MatchStorageFacade.GetAllMatches())
        {
            match.Finish();
        }
        //Это нужно для того, чтобы после остановки unity проекта потоки остановились и не писали в консоль.
        gameServer.StopListeningThreads();
    }
}
