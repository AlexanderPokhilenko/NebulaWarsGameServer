﻿using Entitas;

public class AISystems : Feature
{
    public AISystems(Contexts contexts) : base("AI Systems")
    {
        Add(new TargetDetectionSystem(contexts));
        Add(new TargetShootingSystem(contexts));
        Add(new DirectionTargetingSystem(contexts));
        Add(new RemoveTargetForChangingSystem(contexts));
        Add(new RemoveDistantTargetSystem(contexts));
    }
}