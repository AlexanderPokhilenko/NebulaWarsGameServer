using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StubSpawner : MonoBehaviour
{
    public BaseObject prototype;
    public Vector2 position;
    public float direction;
    public bool spawnOne;

    private GameContext gameContext;


    // Start is called before the first frame update
    void Start()
    {
        gameContext = Contexts.sharedInstance.game;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnOne && prototype != null)
        {
            var entity = prototype.CreateEntity(gameContext, position, direction);
            if(entity.isNotDecelerating && entity.hasMaxVelocity && !entity.isUnmovable) entity.AddVelocity(Vector2.right.GetRotated(direction));
            spawnOne = false;
        }
    }
}
