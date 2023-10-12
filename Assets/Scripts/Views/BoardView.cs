using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Match3;
using UnityEngine;
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
            var width = board.Tiles.GetLength(0);
            var height = board.Tiles.GetLength(1);
            _tileViews = new TileView[width, height];

            var showingSequence = DOTween.Sequence();
            for (var x = 0; x < width; x++)
            {
                for (var y = height - 1; y >= 0; y--)
                {
                    showingSequence.Join(ShowNewTile(board, x, y));
                }
            }

            return showingSequence.ToUniTask();
        }

        private Tweener ShowNewTile(Board board, int x, int y)
        {
            var tileSettings = board.Tiles[x, y];
            
            // TODO: I understand that it's critically important to use a pool here,
            // but in this case, I'm not doing it jut to save time
            var tile = Instantiate(tileSettings.Tile, transform);
            var tileTransform = tile.transform;

            var tileIndex = new Vector3Int(x, y);
            tileTransform.position = _grid.GetCellCenterWorld(tileIndex);
            tileTransform.localScale = Vector3.zero;
            
            _tileViews[x, y] = tile;
            
            return tile.transform.DOScale(Vector3.one, _appearingAnimationDuration);
        }

        public UniTask ShowSuccessfulSwapAnimation(Vector3Int first, Vector3Int second)
        {
            var firstTile = _tileViews[first.x, first.y];
            var secondTile = _tileViews[second.x, second.y];

            return DOTween.Sequence()
                .Join(secondTile.transform.DOMove(firstTile.transform.position, _successfulSwapAnimationDuration))
                .Join(firstTile.transform.DOMove(secondTile.transform.position, _successfulSwapAnimationDuration))
                .OnComplete(() => _tileViews.Swap(first, second))
                .ToUniTask();
        }

        public UniTask ShowFailSwapAnimation(Vector3Int first, Vector3Int second)
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

            for (var i = 0; i < _board.Tiles.GetLength(0); i++)
            {
                for (var j = 0; j < _board.Tiles.GetLength(1); j++)
                {
                    var tile = _board.Tiles[i, j];
                    if (tile != null)
                    {
                        continue;
                    }

                    if (_tileViews[i, j] == null)
                    {
                        continue;
                    }

                    var tileView = _tileViews[i, j];
                    // TODO: return the view to the pool
                    sequence.Join(tileView.transform.DOScale(Vector3.zero, _removalAnimationDuration));
                    
                    _tileViews[i, j] = null;
                }
            }

            return sequence.ToUniTask();
        }

        public UniTask ShowFallAnimation()
        {
            var fallSequence = DOTween.Sequence();
            
            // TODO: this place looks quite similar to BoardService.FallTilesInColumn 
            // I can get rid of it, but for this I need to create some TileModel with its position
            // and make my view just following this model
            for (var x = 0; x < _tileViews.GetLength(0); x++)
            {
                var countEmptyTiles = 0;
                var height = _tileViews.GetLength(1);
                for (var y = height - 1; y >= 0; y--)
                {
                    var tileView = _tileViews[x, y];
                    if (tileView == null)
                    {
                        countEmptyTiles++;
                    }
                    else if (countEmptyTiles > 0)
                    {
                        _tileViews[x, y + countEmptyTiles] = _tileViews[x, y];
                        _tileViews[x, y] = null;
                        
                        var currentPosition = tileView.transform.position;
                        var newIndex = new Vector3Int(x, y + countEmptyTiles);
                        var targetPosition = _grid.GetCellCenterWorld(newIndex);
                        var distance = Vector3.Distance(currentPosition, targetPosition);
                        var duration = distance * _fallDurationPerMeter;

                        fallSequence.Join(tileView.transform.DOMove(targetPosition, duration));
                    }
                }
            }

            return fallSequence.ToUniTask();
        }

        public UniTask ShowNewTiles()
        {
            var sequence = DOTween.Sequence();
            
            var width = _board.Tiles.GetLength(0);
            var height = _board.Tiles.GetLength(1);
            for (var x = 0; x < width; x++)
            {
                for (var y = height - 1; y >= 0; y--)
                {
                    if (_tileViews[x, y] == null)
                    {
                        sequence.Join(ShowNewTile(_board, x, y));
                    }
                }
            }

            return sequence.ToUniTask();
        }
    }
}