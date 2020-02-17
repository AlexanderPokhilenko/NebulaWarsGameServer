﻿using NetworkLibrary.NetworkLibrary.Http;

//TODO заменить GameRoomData на обёртку

namespace Server.Http
{
    public class HttpMessageHandlers
    {
        private readonly BattleCreatorHandler battleCreator=new BattleCreatorHandler();

        public GameRoomValidationResult Handle(GameRoomData roomData)
        {
            return battleCreator.Handle(roomData);
        }
    }
}