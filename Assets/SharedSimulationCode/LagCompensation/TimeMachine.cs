// using Entitas;
//
// namespace SharedSimulationCode.LagCompensation
// {
//     public class TimeMachine : ITimeMachine
//     {
//         //История игровых состояний
//         private readonly IGameStateHistory history;
//         //Набор систем, расставляющих коллайдеры в физическом мире 
//         //по данным из игрового состояния
//         private readonly ISystem[] inputToTransformSystems;
//         //Текущее игровое состояние на сервере
//         private readonly GameState presentState;
//
//         public TimeMachine(IGameStateHistory history, GameState presentState, ISystem[] timeInitInputToTransformSystems)
//         {
//             this.history = history; 
//             this.presentState = presentState;
//             inputToTransformSystems = timeInitInputToTransformSystems;  
//         }
//
//         public GameState TravelToTime(int tick)
//         {
//             GameState pastState;
//             if (tick == presentState.time)
//             {
//                 pastState = presentState;
//             }
//             else
//             {
//                 pastState = history.Get(tick);
//             }
//             
//             foreach (var system in inputToTransformSystems)
//             {
//                 // system.Execute(pastState);
//             }
//             return pastState;
//         }
//     }
// }