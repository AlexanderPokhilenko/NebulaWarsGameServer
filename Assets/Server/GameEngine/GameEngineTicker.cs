using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class GameEngineTicker
    {
        private readonly MatchStorage matchStorage;
        private readonly MatchLifeCycleManager matchLifeCycleManager;
        
        public GameEngineTicker(MatchStorage matchStorage, MatchLifeCycleManager matchLifeCycleManager)
        {
            this.matchStorage = matchStorage;
            this.matchLifeCycleManager = matchLifeCycleManager;
        }

        public void Tick()
        {
            //Создание сущностей ввода игроков
            StaticInputMessagesSorter.Spread();
            StaticExitMessageSorter.Spread();
            
            //Перемещение игровых сущностей
            foreach (var match in matchStorage.DichGetMatches())
            {
                match.Execute();
                match.Cleanup();
            }

            //создание/удаление матчей
            matchLifeCycleManager.UpdateMatchesLifeStatus();
        }
    }
}