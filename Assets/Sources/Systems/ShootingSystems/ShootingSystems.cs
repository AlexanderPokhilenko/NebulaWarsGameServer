internal class ShootingSystems : Feature
{
    public ShootingSystems(Contexts contexts) : base("Shooting Systems")
    {
        Add(new RemovingNegativeCannonCooldownSystem(contexts));
        Add(new CannonShootingSystem(contexts));
        Add(new AbilityUsingSystem(contexts));
        Add(new AddingCannonCooldownSystem(contexts));
        Add(new CannonShootRemoverSystem(contexts));
        Add(new AbilityUsageRemoverSystem(contexts));
    }
}
