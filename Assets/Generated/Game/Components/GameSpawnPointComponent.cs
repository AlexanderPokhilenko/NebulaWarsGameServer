//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public SpawnPointComponent spawnPoint { get { return (SpawnPointComponent)GetComponent(GameComponentsLookup.SpawnPoint); } }
    public bool hasSpawnPoint { get { return HasComponent(GameComponentsLookup.SpawnPoint); } }

    public void AddSpawnPoint(UnityEngine.Vector3 newPosition, UnityEngine.Quaternion newRotation) {
        var index = GameComponentsLookup.SpawnPoint;
        var component = (SpawnPointComponent)CreateComponent(index, typeof(SpawnPointComponent));
        component.position = newPosition;
        component.rotation = newRotation;
        AddComponent(index, component);
    }

    public void ReplaceSpawnPoint(UnityEngine.Vector3 newPosition, UnityEngine.Quaternion newRotation) {
        var index = GameComponentsLookup.SpawnPoint;
        var component = (SpawnPointComponent)CreateComponent(index, typeof(SpawnPointComponent));
        component.position = newPosition;
        component.rotation = newRotation;
        ReplaceComponent(index, component);
    }

    public void RemoveSpawnPoint() {
        RemoveComponent(GameComponentsLookup.SpawnPoint);
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

    static Entitas.IMatcher<GameEntity> _matcherSpawnPoint;

    public static Entitas.IMatcher<GameEntity> SpawnPoint {
        get {
            if (_matcherSpawnPoint == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.SpawnPoint);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherSpawnPoint = matcher;
            }

            return _matcherSpawnPoint;
        }
    }
}