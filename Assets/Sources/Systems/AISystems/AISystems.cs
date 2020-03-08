using Entitas;

public class AISystems : Feature
{
    public AISystems(Contexts contexts) : base("AI Systems")
    {
        Add(new BotsMovingSystem(contexts));
        Add(new TargetPointMovingSystem(contexts));
        Add(new TargetDetectionSystem(contexts));
        Add(new TargetShootingSystem(contexts));
        Add(new DirectionShootingSystem(contexts));
        Add(new DirectionTargetingSystem(contexts));
        Add(new AngularVelocityTargetingSystem(contexts));
        Add(new RemoveTargetForChangingSystem(contexts));
        Add(new RemoveTargetForSingleTargetingSystem(contexts));
        Add(new RemoveDistantTargetSystem(contexts));
    }
}