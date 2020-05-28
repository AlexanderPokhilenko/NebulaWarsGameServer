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
        TestMessageClass1 test =new TestMessageClass1();
        ZeroFormatterSerializer.Serialize(test);
        
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
