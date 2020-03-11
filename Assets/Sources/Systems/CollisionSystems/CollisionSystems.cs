﻿using Entitas;

public class CollisionSystems : Feature
{
    public CollisionSystems(Contexts contexts) : base("Collision Systems")
    {
        Add(new AddCollidersToRectSystem(contexts));
        Add(new AddCollidersToPathSystem(contexts));
        Add(new AddCollidersToCircleSystem(contexts));
        Add(new AddGlobalCollisionComponentsSystem(contexts));
        Add(new ConcaveColliderDetectionSystem(contexts));
        Add(new UpdateGlobalCollisionComponentsSystem(contexts));
        Add(new CollisionDetectionSystem(contexts));
        Add(new CollisionFixedPenetrationDetectionSystem(contexts));
        Add(new CollisionPenetrationAvoidanceSystem(contexts));
    }
}