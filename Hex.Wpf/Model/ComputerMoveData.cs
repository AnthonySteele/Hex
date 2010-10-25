namespace Hex.Wpf.Model
{
    using System;
    using Hex.Board;

    public class ComputerMoveData
    {
        public Location Move { get; set; }

        public TimeSpan Time { get; set; }

        public bool IsGameWon { get; set; }

        public bool PlayerX { get; set; }
    }
}