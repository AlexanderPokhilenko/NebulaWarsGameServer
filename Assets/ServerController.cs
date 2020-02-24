using Server;
using Server.Utils;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    void Start()
    {
        Program.Main();
        // RunTests();
    }

    private void RunTests()
    {
        // Test9();
        Log.Info("Test had already worked");
    }

   
    private void Test9()
    {
        string path = "SO/BaseObjects/HarePlayer";
        var playerPrototype = Resources.Load<PlayerObject>(path);
        if (playerPrototype == null)
            Log.Error("path "+path+" not works");
        else
            Log.Info("path " + path + " works");
    }

}
