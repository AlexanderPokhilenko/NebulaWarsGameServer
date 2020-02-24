﻿using System;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Utils;
using UnityEngine;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Спавнит игроков на карте.
    /// </summary>
    public class PlayersInitSystem:IInitializeSystem
    {
        private readonly PlayerObject playerPrototype;
        private readonly GameContext gameContext;
        private readonly GameRoomData roomData;

        public PlayersInitSystem(Contexts contexts, GameRoomData roomData)
        {
            gameContext = contexts.game;
            this.roomData = roomData;
            playerPrototype = Resources.Load<PlayerObject>("SO/BaseObjects/HarePlayer");
            
            if (playerPrototype == null)
                throw new Exception("Не удалось загрузить asset PlayerObject");
        }    
        
        public void Initialize()
        {
            Log.Info($"Создание игроков для игровой комнаты с номером {roomData.GameRoomNumber}");

            try
            {
                
                foreach (var playerInfo in roomData.Players)
                {
                    Log.Info($"Создание игрока с id = {playerInfo.GoogleId} для комнаты {roomData.GameRoomNumber}");

                    if (playerPrototype == null)
                        throw new NullReferenceException(nameof(playerPrototype));

                    var gameEntity = playerPrototype.CreateEntity(gameContext);
                    gameEntity.AddPlayer(/*playerInfo.GoogleId, */playerInfo.TemporaryId);
                    var rndPosition = GetRandomCoordinates();
                    gameEntity.AddPosition(rndPosition);
                    gameEntity.AddDirection(0);
                
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Брошено исключение PlayersInitSystem Initialize "+e.Message);
            }
        }
        
        //x in [-10;10]. y in [-5;5]
        private Vector2 GetRandomCoordinates()
        {
            //var random = new System.Random();
            float x = /*random.Next(-1000, 1000)/100f;*/ UnityEngine.Random.Range(-10f, 10f);
            float y = /*random.Next(-500, 500)/100f;*/ UnityEngine.Random.Range(-5f, 5f);
            var coordinates = new Vector2(x, y);
            return coordinates;
        }
    }
}