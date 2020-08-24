using System.Linq;
using Entitas;

namespace Server.GameEngine.Experimental.Systems
{
    public class ViewAreasInitSystem : IInitializeSystem
    {
        private readonly PlayersViewAreas viewAreas;
        private readonly IGroup<ServerGameEntity> players;

        public ViewAreasInitSystem(Contexts contexts, PlayersViewAreas playersViewAreas)
        {
            viewAreas = playersViewAreas;
            var gameContext = contexts.serverGame;
            players = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
        }

        public void Initialize()
        {
            var playerIds = players.AsEnumerable().Select(player => player.player.playerId);
            viewAreas.Initialize(playerIds);
        }
    }
}