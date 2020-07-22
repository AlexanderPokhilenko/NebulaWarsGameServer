public class AISystems : Feature
{
    public AISystems(Contexts contexts) : base("AI Systems")
    {
        Add(new BotsOnHealthMoveChangingSystem(contexts));
        Add(new BotsMovingSystem(contexts));
        Add(new TargetPointMovingSystem(contexts));

        Add(new DirectionSaverCheckerSystem(contexts));
        Add(new InitialDirectionSaverCheckerSystem(contexts));
        Add(new LocalTargetingSystem(contexts));

        Add(new TargetDetectionSystem(contexts));
        Add(new ChaserTargetPointUpdaterSystem(contexts));

        Add(new TargetShootingSystem(contexts));

        Add(new DirectionShootingSystem(contexts));
        Add(new DirectionTargetingSystem(contexts));
        Add(new AngularVelocityTargetingSystem(contexts));

        Add(new BotAbilityUsingSystem(contexts));

        Add(new BotsFrameMovingControlSystem(contexts));
        Add(new MovingFramesDecreaseSystem(contexts));

        Add(new RemoveTargetForChangingSystem(contexts));
        Add(new RemoveTargetForSingleTargetingSystem(contexts));
        Add(new RemoveDistantTargetSystem(contexts));
    }
}