using Cysharp.Threading.Tasks;
using StateMachine;
using UnityEngine;

namespace GameStates
{
    public class GameOverState : IState
    {
        public void Initialize(StateMachine.StateMachine stateMachine)
        {
        }

        public UniTask Exit()
        {
            // TODO:
            Debug.Log("Game Over");
            return UniTask.CompletedTask;
        }
        
        public UniTask Enter()
        {
            return UniTask.CompletedTask;
        }
    }
}