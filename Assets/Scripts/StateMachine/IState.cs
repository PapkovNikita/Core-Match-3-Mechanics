using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}