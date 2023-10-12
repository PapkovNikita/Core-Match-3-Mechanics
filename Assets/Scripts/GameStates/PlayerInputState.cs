using System;
using Cysharp.Threading.Tasks;
using Services;
using Settings;
using StateMachine;
using UnityEngine;

namespace GameStates
{
    /// <summary>
    /// Player Input State:
    /// - Wait for the player to choose a tile and decide where to swap it.
    /// - This state is active until a valid move is made.
    /// </summary>
    public class PlayerInputState : IState
    {
        private readonly ISwipeHandler _swipeHandler;
        private readonly ILevelSettings _levelSettings;
        private StateMachine.StateMachine _stateMachine;

        public PlayerInputState(ISwipeHandler swipeHandler, ILevelSettingsProvider levelSettingsProvider)
        {
            _levelSettings = levelSettingsProvider.GetCurrentLevelSettings();
            _swipeHandler = swipeHandler;
        }

        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Exit()
        {
            _swipeHandler.Swiped -= OnSwiped;
            return UniTask.CompletedTask;
        }

        public UniTask Enter()
        {
            _swipeHandler.Swiped += OnSwiped;
            return UniTask.CompletedTask;
        }

        private async void OnSwiped(SwipeData swipe)
        {
            if (IsOutsideBoard(swipe.StartIndex))
            {
                return;
            }

            var isSwipeInsideSameTile = swipe.EndIndex == swipe.StartIndex;
            if (isSwipeInsideSameTile)
            {
                return;
            }

            var isVerticalOrHorizontalSwipe = swipe.EndIndex.x == swipe.StartIndex.x ||
                                              swipe.EndIndex.y == swipe.StartIndex.y;
            if (!isVerticalOrHorizontalSwipe)
            {
                return;
            }

            var direction = GetSwipeDirection(swipe);
            var nextTileIndex = swipe.StartIndex + direction;
            if (IsOutsideBoard(nextTileIndex))
            {
                return;
            }

            var context = new SwapStateContext(swipe.StartIndex, nextTileIndex);
            await _stateMachine.Enter<SwapState, SwapStateContext>(context);
        }

        private static Vector3Int GetSwipeDirection(SwipeData swipe)
        {
            var directionY = Math.Sign(swipe.EndIndex.y - swipe.StartIndex.y);
            var directionX = Math.Sign(swipe.EndIndex.x - swipe.StartIndex.x);
            var direction = new Vector3Int(directionX, directionY);
            return direction;
        }

        private bool IsOutsideBoard(Vector3Int index)
        {
            return index.x < 0 || index.x >= _levelSettings.Size.x ||
                   index.y < 0 || index.y >= _levelSettings.Size.y;
        }
    }
}