namespace Server.GameEngine.Experimental
{
    public class ContainerRudpSender:IRudpSender
    {
        private MatchStorage matchStorage;

        public ContainerRudpSender(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public void SendUnconfirmedMessages()
        {
            //TODO достать все неподтверждённые сообщения 
            //упаковать в контейнеры и отправить
        }
    }
}