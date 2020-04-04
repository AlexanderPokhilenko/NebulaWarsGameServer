// #define USE_OLD_INIT_SYSTEMS

using System;
using System.Collections.Generic;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using UnityEngine;

namespace Server.GameEngine
{
    public class Match
    {
        private Entitas.Systems systems;
        public Contexts Contexts { get; private set; }
        public BattleRoyaleMatchData matchData { get; private set; }
        private DateTime? gameStartTime;

        private readonly MatchStorageFacade matchStorageFacade;

        // private readonly FlameCircleObject zoneObject;
        private readonly Dictionary<int, (int playerId, ViewTypeId type)> possibleKillersInfo;

        private bool gameOver;
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(Match));
        
        public Match(MatchStorageFacade matchStorageFacade)
        {
            this.matchStorageFacade = matchStorageFacade;
            possibleKillersInfo = new Dictionary<int, (int playerId, ViewTypeId type)>();
            //TODO: как-то обойтись без использования AssetDatabase; добавить возможность менять параметры зоны для разных карт
            // zoneObject = Resources.Load<FlameCircleObject>("SO/BaseObjects/FlameCircle");
        }

        public void ConfigureSystems(BattleRoyaleMatchData matchDataArg)
        {
            Log.Info("Создание новой комнаты номер = "+matchDataArg.MatchId);

            matchData = matchDataArg;
            Contexts = ContextsPool.GetContexts();
            Contexts.SubscribeId();
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(Contexts);
            Log.Info("Количество контекстов в списке CollidersDrawer'а: " + CollidersDrawer.contextsList.Count);
#endif
            systems = new Entitas.Systems()
#if USE_OLD_INIT_SYSTEMS
                    .Add(new ZoneInitSystem(Contexts, zoneObject))
                    .Add(new PlayersInitSystem(Contexts, matchDataArg))
                    .Add(new AsteroidsInitSystem(Contexts))
                    .Add(new SpaceStationsInitSystem(Contexts))
                    .Add(new BonusesInitSystem(Contexts))
#else
                    .Add(new MapInitSystem(Contexts, matchDataArg))
#endif
                    
                    // .Add(new TestEndMatchSystem2(Contexts))
                    
                    .Add(new MatchDataInitSystem(Contexts, matchDataArg))
                    .Add(new PlayerMovementHandlerSystem(Contexts))
                    .Add(new PlayerAttackHandlerSystem(Contexts))
                    .Add(new ParentsSystems(Contexts))
                    .Add(new AISystems(Contexts))
                    .Add(new MovementSystems(Contexts))
                    .Add(new GlobalTransformSystem(Contexts))
                    .Add(new ShootingSystems(Contexts))
                    .Add(new CollisionSystems(Contexts))
                    .Add(new EffectsSystems(Contexts))
                    .Add(new TimeSystems(Contexts))
                    .Add(new UpdatePossibleKillersSystem(Contexts, possibleKillersInfo))
                    .Add(new NetworkKillsSenderSystem(Contexts, possibleKillersInfo))
                    .Add(new FinishMatchSystem(Contexts, this))
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    //TODO важно
                    .Add(new DestroySystems(Contexts))
                    //TODO важно
                    .Add(new NetworkSenderSystem(Contexts))
                    .Add(new MaxHpUpdaterSystem(Contexts))
                    .Add(new ShieldPointsUpdaterSystem(Contexts))
                    .Add(new InputDeletingSystem(Contexts))
                    .Add(new GameDeletingSystem(Contexts))
                ;

            systems.ActivateReactiveSystems();
            systems.Initialize();
            gameStartTime = DateTime.UtcNow;
        }
        
        public void Execute()
        {
            if (gameOver) return;
            if (IsSessionTimedOut())
            {
                FinishGame();
                return;
            }
            systems.Execute();
        }

        public void Cleanup()
        {
            if (gameOver) return;
            systems.Cleanup();
        }

        public void TearDown()
        {
            systems.DeactivateReactiveSystems();
            systems.TearDown();
            systems.ClearReactiveSystems();
            Contexts.UnsubscribeId();
            ContextsPool.RetrieveContexts(Contexts);
            possibleKillersInfo.Clear();
        }

        private bool IsSessionTimedOut()
        {
            if (gameStartTime == null) return false;
            var gameDuration = DateTime.UtcNow - gameStartTime.Value;
            return gameDuration > GameSessionGlobals.GameDuration;
        }

        public void FinishGame()
        {
            Log.Error(nameof(FinishGame));
            gameOver = true;
            matchStorageFacade.MarkBattleAsFinished(matchData.MatchId);
        }
    }
}