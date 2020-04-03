namespace Server.GameEngine.Experimental
{
    public class ContainerRudpSender:IRudpSender
    {
        private MatchStorageFacade matchStorageFacade;

        public ContainerRudpSender(MatchStorageFacade matchStorageFacade)
        {
            this.matchStorageFacade = matchStorageFacade;
        }
        
        public void SendUnconfirmedMessages()
        {
            //TODO достать все неподтверждённые сообщения 
            //упаковать в контейнеры и отправить
        }
    }
}