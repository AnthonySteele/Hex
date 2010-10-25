namespace Hex.Engine.CandiateMoves
{
    using System.Collections.Generic;
    using Hex.Board;

    public interface ICandidateMoves
    {
        IEnumerable<Location> CandidateMoves(HexBoard board, int lookaheadDepth);
    }
}
