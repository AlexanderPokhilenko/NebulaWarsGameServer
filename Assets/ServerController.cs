using Server;
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
        gameServer.StopListeningThreads();
    }
}
