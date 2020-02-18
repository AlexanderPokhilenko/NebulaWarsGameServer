using NetworkLibrary.NetworkLibrary.Http;

namespace Server.Http
{
    public class HttpMessageHandlers
    {
        private readonly BattleCreator battleCreator=new BattleCreator();

        public GameRoomValidationResult Handle(GameRoomData roomData)
        {
            return battleCreator.Handle(roomData);
        }
    }
}