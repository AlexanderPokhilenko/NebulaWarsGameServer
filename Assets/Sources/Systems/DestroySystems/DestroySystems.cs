internal class DestroySystems : Feature
{
    public DestroySystems(Contexts contexts) : base("Destroy Systems")
    {
        Add(new LowHealthSystem(contexts));
        Add(new LowLifetimeSystem(contexts));
        Add(new CollapsingSystem(contexts));
        Add(new ParentDependentDeletingSystem(contexts));
        Add(new ParentLinkDeletingSystem(contexts));
        Add(new TargetLinkDeletingSystem(contexts));
        Add(new DropSystem(contexts));
        Add(new UpgradesDropSystem(contexts));
        Add(new DeleteSystem(contexts));
    }
}
