using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IPaylodedState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload payload);
    }
}