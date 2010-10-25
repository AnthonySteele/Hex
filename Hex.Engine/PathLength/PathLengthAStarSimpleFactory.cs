namespace Hex.Engine.PathLength
{
    using Board;

    public class PathLengthAStarSimpleFactory : IPathLengthFactory
    {
        public PathLengthBase CreatePathLength(HexBoard board)
        {
            return new PathLengthAStar(board)
                {
                    UseNeighbours2 = false
                };
        }
    }
}