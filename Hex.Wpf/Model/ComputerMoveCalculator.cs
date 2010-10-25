namespace Hex.Wpf.Model
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;

    using Hex.Board;
    using Hex.Engine;
    using Hex.Engine.CandiateMoves;
    using Hex.Engine.Lookahead;

    public class ComputerMoveCalculator
    {
        private readonly HexGame hexGame;
        private readonly Action<ComputerMoveData> completedAction;
        private readonly int computerSkillLevel;

        public ComputerMoveCalculator(HexGame hexGame, Action<ComputerMoveData> completedAction, int computerSkillLevel)
        {
            this.hexGame = hexGame;
            this.completedAction = completedAction;
            this.computerSkillLevel = computerSkillLevel;
        }

        public Location PlayLocation { get; private set; }

        public TimeSpan MoveTime { get; private set; }

        public bool IsGameWon { get; private set; }

        public Occupied ForcePlayer { get; set; }

        public void Move()
        {
            Task doMove = new Task(this.DoMove);
            doMove.Start();
        }

        private void DoMove()
        {
            Minimax hexPlayer = new Minimax(this.hexGame.Board, this.hexGame.GoodMoves, this.MakeCandiateMovesFinder());
            MinimaxResult minimaxResult = hexPlayer.DoMinimax(this.computerSkillLevel, this.MoveIsPlayerX());
            this.PlayLocation = this.GetBestMove(minimaxResult);
            this.MoveTime = hexPlayer.MoveTime;

            this.CallCompletedAction();
        }

        private bool MoveIsPlayerX()
        {
            switch (this.ForcePlayer)
            {
                case Occupied.PlayerX:
                    return true;
                case Occupied.PlayerY:
                    return false;
                default:
                    return this.hexGame.PlayerX;
            }
        }

        private void CallCompletedAction()
        {
            if (this.completedAction != null)
            {
                ComputerMoveData result = new ComputerMoveData
                    {
                        Move = this.PlayLocation,
                        Time = this.MoveTime,
                        IsGameWon = this.IsGameWon,
                        PlayerX = this.MoveIsPlayerX()
                    };
                Action callWrapper = () => this.completedAction(result);
                Application.Current.Dispatcher.BeginInvoke(callWrapper);
            }
        }

        private Location GetBestMove(MinimaxResult playResult)
        {
            Location result = playResult.Move;

            int moveScore = playResult.Score;
            this.IsGameWon = MoveScoreConverter.IsWin(moveScore) && MoveScoreConverter.WinDepth(moveScore) == 1;

            Occupied opponent = (! this.hexGame.PlayerX).ToPlayer();
            bool losingMove = MoveScoreConverter.Winner(playResult.Score) == opponent;
            if (losingMove)
            {
                Location losingLocation = this.MakeLosingMove();
                if (losingLocation != Location.Null)
                {
                    result = losingLocation;
                }
            }

            return result;
        }

        private ICandidateMoves MakeCandiateMovesFinder()
        {
            return new CandidateMovesAllSorted(this.hexGame.GoodMoves, this.hexGame.CountCellsPlayed);
        }

        private Location MakeLosingMove()
        {
            List<Location> candidates = this.hexGame.GetCleanPathIntersection();

            if (candidates.Count > 0)
            {
                // pick one from the list at random
                Random randoms = new Random();
                int randomIndex = randoms.Next(candidates.Count);
                return candidates[randomIndex];
            }

            return Location.Null;
        }
    }
}
