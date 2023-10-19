using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IStateMachine
    {
        UniTask Enter<TState>() where TState : class, IState;
        UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>;
    }
}