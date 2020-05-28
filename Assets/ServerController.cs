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
        //Arrange
        ViewTransform viewTransform = new ViewTransform(45.25f, -4.3f, 180f, ViewTypeId.Asteroid100);
            
        //Act
        var data = ZeroFormatterSerializer.Serialize(viewTransform);
        var viewTransform2 = ZeroFormatterSerializer.Deserialize<ViewTransform>(data);

        Debug.LogWarning(viewTransform2.X);
        Debug.LogWarning(viewTransform2.Y);
        Debug.LogWarning(viewTransform2.Angle);

        
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
