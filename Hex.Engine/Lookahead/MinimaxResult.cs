using System;

namespace Hex.Engine.Lookahead
{
    using Hex.Board;

    public class MinimaxResult
    {
        public MinimaxResult(int score)
        {
            this.Move = Location.Null;
            this.Score = score;
        }

        public MinimaxResult(Location move, MinimaxResult nextMove)
        {
            this.Move = move;
            this.Score = nextMove.Score;
            this.NextMove = nextMove;
        }

        public Location Move { get; set; }

        public int Score { get; set; }

        public int AlphaBeta { get; set; }

        public MinimaxResult NextMove { get; set; }

        public override string ToString()
        {
            return string.Format("Move: {0} Score: {1}", this.Move, this.Score);
        }

        public void MoveWins()
        {
            if (MoveScoreConverter.IsWinForPlayer(this.Score, Occupied.PlayerX))
            {
                this.Score--;
            }
            else if (MoveScoreConverter.IsWinForPlayer(this.Score, Occupied.PlayerY))
            {
                this.Score++;
            }
        }
    }
}
