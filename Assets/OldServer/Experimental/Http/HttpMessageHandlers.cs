using NetworkLibrary.NetworkLibrary.Http;

//TODO заменить GameRoomData на обёртку

namespace AmoebaBattleServer01.Experimental.Http
{
    public class HttpMessageHandlers
    {
        private GameSessionCreatorHandler gameSessionCreator=new GameSessionCreatorHandler();

        public GameRoomValidationResult Handle(GameRoomData roomData)
        {
            return gameSessionCreator.Handle(roomData);
        }
    }
}