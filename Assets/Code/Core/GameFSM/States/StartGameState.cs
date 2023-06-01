using System;
using Cysharp.Threading.Tasks;

namespace Snake.Core.GameFSM.States
{
    public class GameStartState : IState
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