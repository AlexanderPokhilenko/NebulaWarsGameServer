using Server;
using Server.GameEngine;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    private GameServer gameServer;
    
    private void Start()
    {
        gameServer = new GameServer();
        gameServer.Run();
    }

    /// <summary>
    /// Это нужно для того, чтобы после остановки unity проекта оно не писало в консоль.
    /// </summary>
    private void OnDestroy()
    {
        foreach (var match in MatchManager.MatchStorageFacade.GetAllGameSessions())
        {
            match.Finish();
        }
        gameServer.StopListeningThreads();
    }
}
