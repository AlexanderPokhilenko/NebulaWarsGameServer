using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class GameEngineTicker
    {
        private readonly MatchStorageFacade matchStorageFacade;
        private readonly MatchLifeCycleManager matchLifeCycleManager;
        
        public GameEngineTicker()
        {
            matchStorageFacade = new MatchStorageFacade();
            matchLifeCycleManager = new MatchLifeCycleManager();
        }

        public void Tick()
        {
            //Создание сущностей ввода игроков
            StaticInputMessagesSorter.Spread();
            StaticExitMessageSorter.Spread();
            
            //Обработка 
            foreach (var match in matchStorageFacade.GetAllMatches())
            {
                match.Execute();
                match.Cleanup();
            }

            //создание/удаление матчей
            matchLifeCycleManager.UpdateMatchesLifeStatus();
        }
    }
}