using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using Snake.Core.GameFSM.Components;
using Snake.Core.GameFSM.States;
using t1az2z.Tools.FDebug;

namespace Snake.Core.GameFSM
{
    public class GameFSM
    {
        public IState CurrentState { get; private set; }
        public TransitionStage Stage { get; private set; } = TransitionStage.None;

        public bool TransitionInProgress => Stage is TransitionStage.Enter or TransitionStage.Exit;
        private Dictionary<Type, IState> _states;
        private EcsWorld _world;
        private EcsEntity _stateEntity;
        
        public async void Init(EcsWorld world)
        {
            _world = world;
            _stateEntity = world.NewEntity();
            
            _states = new Dictionary<Type, IState>
            {
                { typeof(GameStartState), new GameStartState() },
                { typeof(GameplayState), new GameplayState() },
                { typeof(LevelFailedState), new LevelFailedState() },
                { typeof(LevelCompleteState), new LevelCompleteState() }
            };

            await SetState<GameStartState>();
        }

        public async UniTask SetState<T>() where T : IState
        {
            var type = typeof(T);
            
            if (CurrentState != null && CurrentState.GetType() == type)
                return;

            if (TransitionInProgress)
                return;

            if (!_states.TryGetValue(type, out var newState))
            {
                FDebug.Log($"#ERROR: No such state of type {type}#");
                return;
            }

            if (CurrentState != null)
            {
                FDebug.Log($"Exiting {CurrentState.GetType()} state");
                Stage = TransitionStage.Exit;
                await CurrentState.OnExit();
            }
            
            CurrentState = newState;
            FDebug.Log($"Entering {CurrentState.GetType()} state");
            Stage = TransitionStage.Enter;
            
            if (!_stateEntity.IsAlive())
                _stateEntity = _world.NewEntity();
            
            _stateEntity.Get<StateEnterEvent>();
            await CurrentState.OnEnter();
            Stage = TransitionStage.None;
        }
    }

    public enum TransitionStage
    {
        None,
        Enter,
        Exit
    }
}