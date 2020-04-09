//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity matchEntity { get { return GetGroup(GameMatcher.Match).GetSingleEntity(); } }
    public MatchComponent match { get { return matchEntity.match; } }
    public bool hasMatch { get { return matchEntity != null; } }

    public GameEntity SetMatch(int newMatchId) {
        if (hasMatch) {
            throw new Entitas.EntitasException("Could not set Match!\n" + this + " already has an entity with MatchComponent!",
                "You should check if the context already has a matchEntity before setting it or use context.ReplaceMatch().");
        }
        var entity = CreateEntity();
        entity.AddMatch(newMatchId);
        return entity;
    }

    public void ReplaceMatch(int newMatchId) {
        var entity = matchEntity;
        if (entity == null) {
            entity = SetMatch(newMatchId);
        } else {
            entity.ReplaceMatch(newMatchId);
        }
    }

    public void RemoveMatch() {
        matchEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public MatchComponent match { get { return (MatchComponent)GetComponent(GameComponentsLookup.Match); } }
    public bool hasMatch { get { return HasComponent(GameComponentsLookup.Match); } }

    public void AddMatch(int newMatchId) {
        var index = GameComponentsLookup.Match;
        var component = (MatchComponent)CreateComponent(index, typeof(MatchComponent));
        component.MatchId = newMatchId;
        AddComponent(index, component);
    }

    public void ReplaceMatch(int newMatchId) {
        var index = GameComponentsLookup.Match;
        var component = (MatchComponent)CreateComponent(index, typeof(MatchComponent));
        component.MatchId = newMatchId;
        ReplaceComponent(index, component);
    }

    public void RemoveMatch() {
        RemoveComponent(GameComponentsLookup.Match);
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

    static Entitas.IMatcher<GameEntity> _matcherMatch;

    public static Entitas.IMatcher<GameEntity> Match {
        get {
            if (_matcherMatch == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Match);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMatch = matcher;
            }

            return _matcherMatch;
        }
    }
}
