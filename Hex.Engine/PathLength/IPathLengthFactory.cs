namespace Hex.Engine.PathLength
{
    using Hex.Board;

    public interface IPathLengthFactory
    {
        PathLengthBase CreatePathLength(HexBoard board);
    }
}
