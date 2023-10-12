using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states = new();
        private IExitableState _currentState;

        protected StateMachine(params IExitableState[] states)
        {
            foreach (var state in states)
            {
                state.Initialize(this);
                _states.Add(state.GetType(), state);
            }
        }

        public async UniTask Enter<TState>() where TState : class, IState
        {
            var newState = await ChangeState<TState>();
            await newState.Enter();
        }

        public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>
        {
            TState newState = await ChangeState<TState>();
            await newState.Enter(payload);
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState
        {
            _states.Add(typeof(TState), state);
        }

        private async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            if (_currentState != null)
            {
                await _currentState.Exit();
            }
      
            var state = GetState<TState>();
            _currentState = state;
      
            return state;
        }
    
        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}