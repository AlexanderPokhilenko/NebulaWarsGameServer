using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class GameEngineTicker
    {
        private readonly InputEntitiesCreator inputEntitiesCreator;
        private readonly MatchStorage matchStorage;
        private readonly MatchLifeCycleManager matchLifeCycleManager;
        private readonly ExitEntitiesCreator exitEntitiesCreator;
        
        public GameEngineTicker(MatchStorage matchStorage, MatchLifeCycleManager matchLifeCycleManager,
            InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator)
        {
            this.matchStorage = matchStorage;
            this.matchLifeCycleManager = matchLifeCycleManager;
            this.inputEntitiesCreator = inputEntitiesCreator;
            this.exitEntitiesCreator = exitEntitiesCreator;
        }

        public void Tick()
        {
            inputEntitiesCreator.Create();
            exitEntitiesCreator.Create();
            
            //Перемещение игровых сущностей
            foreach (Match match in matchStorage.GetAllMatches())
            {
                match.Tick();
            }

            //создание/удаление матчей
            matchLifeCycleManager.UpdateMatchesLifeStatus();
        }
    }
}