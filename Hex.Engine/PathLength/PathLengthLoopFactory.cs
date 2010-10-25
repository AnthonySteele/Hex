namespace Hex.Engine.PathLength
{
    using Hex.Board;

    public class PathLengthLoopFactory : IPathLengthFactory
    {
        public PathLengthBase CreatePathLength(HexBoard board)
        {
            return new PathLengthLoop(board);
        }
    }
}