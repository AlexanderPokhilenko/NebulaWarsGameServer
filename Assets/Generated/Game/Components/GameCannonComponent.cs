//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public CannonComponent cannon { get { return (CannonComponent)GetComponent(GameComponentsLookup.Cannon); } }
    public bool hasCannon { get { return HasComponent(GameComponentsLookup.Cannon); } }

    public void AddCannon(UnityEngine.Vector2 newPosition, float newCooldown, BulletObject newBullet) {
        var index = GameComponentsLookup.Cannon;
        var component = (CannonComponent)CreateComponent(index, typeof(CannonComponent));
        component.position = newPosition;
        component.cooldown = newCooldown;
        component.bullet = newBullet;
        AddComponent(index, component);
    }

    public void ReplaceCannon(UnityEngine.Vector2 newPosition, float newCooldown, BulletObject newBullet) {
        var index = GameComponentsLookup.Cannon;
        var component = (CannonComponent)CreateComponent(index, typeof(CannonComponent));
        component.position = newPosition;
        component.cooldown = newCooldown;
        component.bullet = newBullet;
        ReplaceComponent(index, component);
    }

    public void RemoveCannon() {
        RemoveComponent(GameComponentsLookup.Cannon);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherCannon;

    public static Entitas.IMatcher<GameEntity> Cannon {
        get {
            if (_matcherCannon == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Cannon);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCannon = matcher;
            }

            return _matcherCannon;
        }
    }
}
