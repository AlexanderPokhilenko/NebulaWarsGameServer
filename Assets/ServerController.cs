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

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        foreach (var match in MatchManager.MatchStorageFacade.GetAllMatches())
        {
            match.Finish();
        }
        //Это нужно для того, чтобы после остановки unity проекта оно не писало в консоль.
        gameServer.StopListeningThreads();
    }
}
