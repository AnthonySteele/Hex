namespace Hex.Engine.PathLength
{
    using Board;

    public class PathLengthAStarFactory : IPathLengthFactory
    {
        public PathLengthBase CreatePathLength(HexBoard board)
        {
            return new PathLengthAStar(board)
                {
                    UseNeighbours2 = true
                };
        }
    }
}