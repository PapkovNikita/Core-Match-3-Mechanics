using System;
using Common.Extensions;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer.Unity;

namespace Services
{
    public interface ISwipeHandler : ITickable
    {
        public event Action<SwipeData> Swiped;
    }
    public class SwipeHandler : ISwipeHandler
    {
        public event Action<SwipeData> Swiped;
        
        private readonly Grid _grid;
        private bool _isSwipeStarted;
        private SwipeData _currentSwipe;

        public SwipeHandler(Grid grid)
        {
            _grid = grid;
            Assert.IsNotNull(Camera.main, "Camera.main is null!");
        }

        public void Tick()
        {
            if (!_isSwipeStarted && Input.GetMouseButtonDown(0))
            {
                var startPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
                _currentSwipe = new SwipeData
                {
                    StartIndex = _grid.WorldToCell(startPosition).ToVector2Int()
                };
                
                _isSwipeStarted = true;
            }

            if (_isSwipeStarted && Input.GetMouseButtonUp(0))
            {
                var endPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
                _currentSwipe.EndIndex = _grid.WorldToCell(endPosition).ToVector2Int();
                Swiped?.Invoke(_currentSwipe);
                
                _isSwipeStarted = false;
            }
        }
    }
}