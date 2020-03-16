using Entitas;

namespace Server.GameEngine.Systems
{
    public class GameDeletingSystem : ITearDownSystem
    {
        private readonly GameContext gameContext;

        public GameDeletingSystem(Contexts contexts)
        {
            gameContext = contexts.game;
        }

        public void TearDown()
        {
            gameContext.DestroyAllEntities();
        }
    }
}