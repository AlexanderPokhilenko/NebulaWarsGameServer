//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ChaserComponent chaser { get { return (ChaserComponent)GetComponent(GameComponentsLookup.Chaser); } }
    public bool hasChaser { get { return HasComponent(GameComponentsLookup.Chaser); } }

    public void AddChaser(float newDistance) {
        var index = GameComponentsLookup.Chaser;
        var component = (ChaserComponent)CreateComponent(index, typeof(ChaserComponent));
        component.distance = newDistance;
        AddComponent(index, component);
    }

    public void ReplaceChaser(float newDistance) {
        var index = GameComponentsLookup.Chaser;
        var component = (ChaserComponent)CreateComponent(index, typeof(ChaserComponent));
        component.distance = newDistance;
        ReplaceComponent(index, component);
    }

    public void RemoveChaser() {
        RemoveComponent(GameComponentsLookup.Chaser);
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

    static Entitas.IMatcher<GameEntity> _matcherChaser;

    public static Entitas.IMatcher<GameEntity> Chaser {
        get {
            if (_matcherChaser == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Chaser);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherChaser = matcher;
            }

            return _matcherChaser;
        }
    }
}
