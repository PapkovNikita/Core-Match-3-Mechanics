using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Events;
using Extensions.UniRx;
using Services;
using UniTaskPubSub;

namespace Views
{
    public class BoardPresenter : IDisposable
    {
        private readonly AsyncMessageBus _messageBus;
        private readonly BoardView _boardView;
        
        private CompositeDisposable _subscriptions;

        public BoardPresenter(BoardView boardView, AsyncMessageBus messageBus)
        {
            _messageBus = messageBus;
            _boardView = boardView;
        }

        public void Show()
        {
            _subscriptions = new CompositeDisposable
            {
                _messageBus.Subscribe<BoardInitializedEvent>(OnBoardInitialized),
                _messageBus.Subscribe<TilesSwappedEvent>(OnTilesSwapped),
                _messageBus.Subscribe<TilesRemovedEvent>(OnTilesRemoved),
                _messageBus.Subscribe<TilesFellDownEvent>(OnTilesFellDown),
                _messageBus.Subscribe<NewTilesGeneratedEvent>(OnNewTilesGenerated),
            };
        }

        private UniTask OnNewTilesGenerated(NewTilesGeneratedEvent eventData)
        {
            return _boardView.ShowNewTiles();
        }

        private UniTask OnTilesFellDown(TilesFellDownEvent eventData)
        {
            return _boardView.ShowFallAnimation();
        }

        private UniTask OnTilesRemoved(TilesRemovedEvent eventData)
        {
            return _boardView.ShowRemovalAnimation();            
        }

        private UniTask OnTilesSwapped(TilesSwappedEvent eventData)
        {
            return eventData.Successful ? 
                _boardView.ShowSuccessfulSwapAnimation(eventData.First, eventData.Second) :
                _boardView.ShowFailSwapAnimation(eventData.First, eventData.Second);
        }
        
        private UniTask OnBoardInitialized(BoardInitializedEvent eventData)
        {           
            return _boardView.Show(eventData.Board);
        }
 
        public void Dispose()
        {
            _subscriptions?.Dispose();
        }
    }
}