namespace Hex.Board
{
    /// <summary>
    /// state of a cell - empty, or played by one of the players
    /// </summary>
    public enum Occupied
    {
        /// <summary>
        /// the cell has not been played
        /// </summary>
        Empty = 0, 

        /// <summary>
        /// Played by player X
        /// </summary>
        PlayerX, 

        /// <summary>
        /// Played by player Y
        /// </summary>
        PlayerY
    }
}