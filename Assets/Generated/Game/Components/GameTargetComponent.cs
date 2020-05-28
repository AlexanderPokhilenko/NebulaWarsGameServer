//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public TargetComponent target { get { return (TargetComponent)GetComponent(GameComponentsLookup.Target); } }
    public bool hasTarget { get { return HasComponent(GameComponentsLookup.Target); } }

    public void AddTarget(ushort newId) {
        var index = GameComponentsLookup.Target;
        var component = (TargetComponent)CreateComponent(index, typeof(TargetComponent));
        component.id = newId;
        AddComponent(index, component);
    }

    public void ReplaceTarget(ushort newId) {
        var index = GameComponentsLookup.Target;
        var component = (TargetComponent)CreateComponent(index, typeof(TargetComponent));
        component.id = newId;
        ReplaceComponent(index, component);
    }

    public void RemoveTarget() {
        RemoveComponent(GameComponentsLookup.Target);
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

    static Entitas.IMatcher<GameEntity> _matcherTarget;

    public static Entitas.IMatcher<GameEntity> Target {
        get {
            if (_matcherTarget == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Target);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTarget = matcher;
            }

            return _matcherTarget;
        }
    }
}
