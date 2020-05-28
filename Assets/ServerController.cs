using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server;
using UnityEngine;
using ZeroFormatter;

/// <summary>
/// Точка входа в проект.
/// </summary>
public class ServerController : MonoBehaviour
{
    private Startup startup;

    private void Start()
    {
        // //Arrange
        // float pi = 45.128f;
        //     
        // //Act
        // ushort half = Mathf.FloatToHalf(pi);
        // float restoredPi = Mathf.HalfToFloat(half);
        //
        // Debug.LogWarning(pi);
        // Debug.LogWarning(restoredPi);

        
        startup = new Startup();
        startup.Run();
    }
    
    private void OnDestroy()
    {
        startup.FinishAllMatches();
        
        //Это нужно для того, чтобы после остановки unity проекта потоки остановились и не писали в консоль.
        startup.StopAllThreads();
    }
}
