using Cysharp.Threading.Tasks;

namespace Snake.Core.GameFSM.States
{
    public interface IState
    {
        public UniTask OnEnter();
        public UniTask OnExit();
    }
}