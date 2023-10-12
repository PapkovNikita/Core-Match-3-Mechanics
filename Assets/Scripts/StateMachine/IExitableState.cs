using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IExitableState
    {
        void Initialize(StateMachine stateMachine); 
        UniTask Exit();
    }
}