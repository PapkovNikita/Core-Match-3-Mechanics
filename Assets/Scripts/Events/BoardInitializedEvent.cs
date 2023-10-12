using Services.Board;

namespace Events
{
    public struct BoardInitializedEvent
    {
        public Board Board { get; private set; }

        public BoardInitializedEvent(Board board)
        {
            Board = board;
        }
    }
}