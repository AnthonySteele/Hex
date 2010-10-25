namespace Hex.Wpf.Controls
{
    using System;
    
    using Hex.Board;
    using Hex.Engine;
    using Hex.Wpf.Model;
    
    public class GameSummary
    {
        private readonly HexGame game;

        public GameSummary(HexGame game, GameType gameType)
        {
            this.game = game;
            this.GameType = gameType;
        }

        public GameType GameType { get; private set; }

        public Occupied NextPlayer
        {
            get
            {
                return this.game.PlayerX.ToPlayer();
            }
        }

        public Occupied Winner
        {
            get { return this.game.Winner; }
        }

        public TimeSpan LastMoveDuration { get; set; }

        public string SummaryText
        {
            get
            {
                if (this.game.Winner != Occupied.Empty)
                {
                    return this.WinnerSummary();
                }

                return this.NextPlayerText() + this.MoveCountText();
            }
        }

        public string LastMoveDurationText
        {
            get
            {
                if (this.LastMoveDuration == TimeSpan.Zero)
                {
                    return string.Empty;
                }

                double seconds = this.LastMoveDuration.Seconds + (this.LastMoveDuration.Milliseconds / 1000);
                return string.Format("Computer move completed in {0}:{1:0.0}", this.LastMoveDuration.Minutes, seconds);
            }
        }

        public bool NextMoveIsComputer()
        {
            return this.NextPlayerIsComputer() && this.Winner == Occupied.Empty;
        }

        public bool NextMoveIsHuman()
        {
            return (!this.NextPlayerIsComputer()) && this.Winner == Occupied.Empty;
        }
        
        private string WinnerSummary()
        {
            return string.Format("{0} in {1} moves", this.game.DescribeHasWon(), this.game.CountCellsPlayed);
        }

        private bool NextPlayerIsComputer()
        {
            if (this.NextPlayer == Occupied.PlayerX)
            {
                return this.GameType == GameType.ComputerVersusHuman;
            }

            return this.GameType == GameType.HumanVersusComputer;
        }

        private string MoveCountText()
        {
            return string.Format(" {0} moves played", this.game.CountCellsPlayed);
        }

        private string NextPlayerText()
        {
            if (this.NextPlayer == Occupied.Empty)
            {
                return "No next player.";
            }

            if (this.NextPlayerIsComputer())
            {
                return "Computer " + this.NextPlayerName() + " calculating move.";
            }

            return this.NextPlayerName() + " to play next.";
        }

        private string NextPlayerName()
        {
            return this.NextPlayer == Occupied.PlayerX ? "player X" : "player Y";
        }
    }
}
