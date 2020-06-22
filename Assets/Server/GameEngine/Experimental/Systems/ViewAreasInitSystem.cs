using System.Linq;
using Entitas;

namespace Server.GameEngine.Systems
{
    public class ViewAreasInitSystem : IInitializeSystem
    {
        private readonly PlayersViewAreas viewAreas;
        private readonly IGroup<GameEntity> players;

        public ViewAreasInitSystem(Contexts contexts, PlayersViewAreas playersViewAreas)
        {
            viewAreas = playersViewAreas;
            var gameContext = contexts.game;
            players = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
        }

        public void Initialize()
        {
            var playerIds = players.AsEnumerable().Select(player => player.player.id);
            viewAreas.Initialize(playerIds);
        }
    }
}