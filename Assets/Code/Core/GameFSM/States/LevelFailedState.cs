using System;
using Cysharp.Threading.Tasks;

namespace Snake.Core.GameFSM.States
{
    public class LevelFailedState : IState
    {
        public UniTask OnEnter()
        {
            return UniTask.Delay(TimeSpan.Zero);
        }

        public UniTask OnExit()
        {
            return UniTask.Delay(TimeSpan.Zero);
        }
    }
}