namespace Server.GameEngine.Experimental
{
    public class ContainerRudpSender:IRudpSender
    {
        private BattlesStorage battlesStorage;

        public ContainerRudpSender(BattlesStorage battlesStorage)
        {
            this.battlesStorage = battlesStorage;
        }
        
        public void SendUnconfirmedMessages()
        {
            //TODO достать все неподтверждённые сообщения 
            //упаковать в контейнеры и отправить
        }
    }
}