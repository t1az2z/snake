using Leopotam.Ecs;
using Snake.Core.GameFSM;
using Snake.Core.GameFSM.States;
using UnityEngine;

namespace Snake.Game.Systems
{
    public class DebugKeysSystem : IEcsRunSystem
    {
        private GameFsm _fsm;

        public async void Run()
        {
            if (_fsm.TransitionInProgress)
                return;
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
                await _fsm.SetState<GameplayState>();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                await _fsm.SetState<LevelCompleteState>();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                await _fsm.SetState<LevelFailedState>();
        }
    }
}