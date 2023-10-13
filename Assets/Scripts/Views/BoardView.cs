using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Services.Board;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace Views
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private float _appearingAnimationDuration = 0.5f;
        [SerializeField] private float _successfulSwapAnimationDuration = 0.5f;
        [SerializeField] private float _failSwapAnimationDuration = 0.5f;
        [SerializeField] private float _removalAnimationDuration = 0.5f;
        [SerializeField] private float _fallDurationPerMeter = 0.2f;

        private TileView[,] _tileViews;
        private Grid _grid;
        private Board _board;

        [Inject]
        public void Construct(Grid grid)
        {
            _grid = grid;
        }

        public UniTask Show(Board board)
        {
            _board = board;
            var width = board.GetSize().x;
            var height = board.GetSize().y;
            _tileViews = new TileView[width, height];

            var showingSequence = DOTween.Sequence();
            for (var x = 0; x < width; x++)
            for (var y = height - 1; y >= 0; y--)
            {
                showingSequence.Join(ShowNewTile(board, x, y));
            }

            return showingSequence.ToUniTask();
        }

        private Tweener ShowNewTile(Board board, int x, int y)
        {
            var tileModel = board.GetTileModel(x, y);
            var tileType = tileModel.Type;

            var tile = _pool.Get(tileType, tileType.ViewPrefab);
            var tileTransform = tile.transform;

            var tileIndex = new Vector3Int(x, y);
            tileTransform.position = _grid.GetCellCenterWorld(tileIndex);
            tileTransform.localScale = Vector3.zero;

            tile.Initialize(tileModel);

            _tileViews[x, y] = tile;

            return tile.transform.DOScale(Vector3.one, _appearingAnimationDuration);
        }

        public UniTask ShowSuccessfulSwapAnimation(Vector2Int first, Vector2Int second)
        {
            var firstTile = _tileViews[first.x, first.y];
            var secondTile = _tileViews[second.x, second.y];

            return DOTween.Sequence()
                .Join(secondTile.transform.DOMove(firstTile.transform.position, _successfulSwapAnimationDuration))
                .Join(firstTile.transform.DOMove(secondTile.transform.position, _successfulSwapAnimationDuration))
                .OnComplete(() => _tileViews.Swap(first, second))
                .ToUniTask();
        }

        public UniTask ShowFailSwapAnimation(Vector2Int first, Vector2Int second)
        {
            var firstTile = _tileViews[first.x, first.y];
            var secondTile = _tileViews[second.x, second.y];

            return DOTween.Sequence()
                .Join(secondTile.transform.DOMove(firstTile.transform.position, _failSwapAnimationDuration))
                .Join(firstTile.transform.DOMove(secondTile.transform.position, _failSwapAnimationDuration))
                .SetLoops(2, LoopType.Yoyo)
                .ToUniTask();
        }

        public UniTask ShowRemovalAnimation()
        {
            var sequence = DOTween.Sequence();

            for (var x = 0; x < _tileViews.GetLength(0); x++)
            for (var y = 0; y < _tileViews.GetLength(1); y++)
            {
                var view = _tileViews[x, y];
                if (view == null || !view.Model.IsRemoved)
                {
                    continue;
                }

                Assert.AreEqual(x, view.Model.Position.x);
                Assert.AreEqual(y, view.Model.Position.y);

                var removalAnimation = view.transform
                    .DOScale(Vector3.zero, _removalAnimationDuration)
                    .OnComplete(() => _pool.Release(view.InitialType, view));
                
                sequence.Join(removalAnimation);

                _tileViews[x, y] = null;
            }

            return sequence.ToUniTask();
        }

        public UniTask ShowFallAnimation()
        {
            var fallSequence = DOTween.Sequence();

            for (var x = 0; x < _tileViews.GetLength(0); x++)
            {
                var height = _tileViews.GetLength(1);
                for (var y = height - 1; y >= 0; y--)
                {
                    var tileView = _tileViews[x, y];
                    if (tileView == null)
                    {
                        continue;
                    }
                    
                    var tileModel = tileView.Model;
                    var tilePosition = new Vector2Int(x, y);

                    if (tilePosition == tileModel.Position)
                    {
                        continue;
                    }
                    
                    _tileViews[tileModel.Position.x, tileModel.Position.y] = _tileViews[x, y];
                    _tileViews[x, y] = null;

                    fallSequence.Join(ShowFallAnimation(tileView, tileModel.Position));
                }
            }

            return fallSequence.ToUniTask();
        }

        private Tweener ShowFallAnimation(TileView tileView, Vector2Int position)
        {
            var currentWorldPosition = tileView.transform.position;
            var targetWorldPosition = _grid.GetCellCenterWorld(position.ToVector3Int());
            var distance = Vector3.Distance(currentWorldPosition, targetWorldPosition);
            var duration = distance * _fallDurationPerMeter;

            return tileView.transform.DOMove(targetWorldPosition, duration);
        }

        public UniTask ShowNewTiles()
        {
            var sequence = DOTween.Sequence();

            var width = _board.GetSize().x;
            var height = _board.GetSize().y;

            for (var x = 0; x < width; x++)
            for (var y = height - 1; y >= 0; y--)
            {
                if (_tileViews[x, y] == null)
                {
                    sequence.Join(ShowNewTile(_board, x, y));
                }
            }

            return sequence.ToUniTask();
        }
    }
}