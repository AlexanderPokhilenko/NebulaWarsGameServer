//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public CollisionVectorComponent collisionVector { get { return (CollisionVectorComponent)GetComponent(GameComponentsLookup.CollisionVector); } }
    public bool hasCollisionVector { get { return HasComponent(GameComponentsLookup.CollisionVector); } }

    public void AddCollisionVector(UnityEngine.Vector2 newValue) {
        var index = GameComponentsLookup.CollisionVector;
        var component = (CollisionVectorComponent)CreateComponent(index, typeof(CollisionVectorComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCollisionVector(UnityEngine.Vector2 newValue) {
        var index = GameComponentsLookup.CollisionVector;
        var component = (CollisionVectorComponent)CreateComponent(index, typeof(CollisionVectorComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveCollisionVector() {
        RemoveComponent(GameComponentsLookup.CollisionVector);
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

    static Entitas.IMatcher<GameEntity> _matcherCollisionVector;

    public static Entitas.IMatcher<GameEntity> CollisionVector {
        get {
            if (_matcherCollisionVector == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CollisionVector);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCollisionVector = matcher;
            }

            return _matcherCollisionVector;
        }
    }
}