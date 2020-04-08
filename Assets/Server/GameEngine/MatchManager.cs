using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    //TODO название не очень
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class MatchManager
    {
        public static MatchStorageFacade MatchStorageFacade;
        
        public MatchManager()
        {
            MatchStorageFacade = new MatchStorageFacade();
        }

        public void Tick()
        {
            //Создание сущностей ввода игроков
            StaticInputMessagesSorter.Spread();
            StaticExitMessageSorter.Spread();
            
            //Обработка 
            foreach (var match in MatchStorageFacade.GetAllMatches())
            {
                match.Execute();
                match.Cleanup();
            }

            //создание/удаление матчей
            MatchStorageFacade.UpdateBattlesList();
        }
    }
}