using System.Collections.Generic;
using Services;

namespace GameStates
{
    public struct TileRemovalStateContext
    {
        public List<Match> Matches { get; }

        public TileRemovalStateContext(List<Match> matches)
        {
            Matches = matches;
        }
    }
}