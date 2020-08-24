using Entitas;

namespace Server.GameEngine.Experimental.Systems
{
    public class GameDeletingSystem : ITearDownSystem
    {
        private readonly ServerGameContext gameContext;

        public GameDeletingSystem(Contexts contexts)
        {
            gameContext = contexts.serverGame;
        }

        public void TearDown()
        {
            gameContext.DestroyAllEntities();
        }
    }
}