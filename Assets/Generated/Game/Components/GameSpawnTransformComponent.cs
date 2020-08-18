//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public SpawnTransformComponent spawnTransform { get { return (SpawnTransformComponent)GetComponent(GameComponentsLookup.SpawnTransform); } }
    public bool hasSpawnTransform { get { return HasComponent(GameComponentsLookup.SpawnTransform); } }

    public void AddSpawnTransform(UnityEngine.Transform newTransform) {
        var index = GameComponentsLookup.SpawnTransform;
        var component = (SpawnTransformComponent)CreateComponent(index, typeof(SpawnTransformComponent));
        component.transform = newTransform;
        AddComponent(index, component);
    }

    public void ReplaceSpawnTransform(UnityEngine.Transform newTransform) {
        var index = GameComponentsLookup.SpawnTransform;
        var component = (SpawnTransformComponent)CreateComponent(index, typeof(SpawnTransformComponent));
        component.transform = newTransform;
        ReplaceComponent(index, component);
    }

    public void RemoveSpawnTransform() {
        RemoveComponent(GameComponentsLookup.SpawnTransform);
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

    static Entitas.IMatcher<GameEntity> _matcherSpawnTransform;

    public static Entitas.IMatcher<GameEntity> SpawnTransform {
        get {
            if (_matcherSpawnTransform == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.SpawnTransform);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherSpawnTransform = matcher;
            }

            return _matcherSpawnTransform;
        }
    }
}