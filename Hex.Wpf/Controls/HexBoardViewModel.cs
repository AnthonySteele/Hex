namespace Hex.Wpf.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Hex.Board;
    using Hex.Engine;
    using Hex.Wpf.SelectGame;

    public class HexBoardViewModel : BaseViewModel
    {
        private readonly ObservableCollection<HexCellViewModel> cells = new ObservableCollection<HexCellViewModel>();
        private readonly GameSummary gameSummary;
        private readonly ICommand doComputerMoveCommand;
        private readonly ICommand getDebugDataCommand = new GetDebugDataCommand();
        private readonly int computerSkillLevel;
        private HexGame hexGame;

        public HexBoardViewModel(SelectGameViewModel gameData)
        {
            this.CanPlay = true;
            this.BoardSize = gameData.SelectedBoardSize;

            this.hexGame = new HexGame(this.BoardSize);
            this.Populate();

            this.gameSummary = new GameSummary(this.hexGame, gameData.GameType);

            this.computerSkillLevel = SkillLevelToDepth(gameData.SkillLevel);
            this.doComputerMoveCommand = new DoComputerMoveCommand(this.computerSkillLevel);
            this.CheckComputerMove();
        }

        public HexBoardViewModel(HexBoardViewModel original)
        {
            this.CanPlay = true;
            this.BoardSize = original.BoardSize;

            this.hexGame = new HexGame(this.BoardSize);
            this.hexGame.Board.CopyStateFrom(original.hexGame.Board);

            this.Populate();
            this.CopyCellStates(original.Cells);

            this.gameSummary = new GameSummary(this.hexGame, original.gameSummary.GameType);
            this.computerSkillLevel = original.computerSkillLevel;
            this.doComputerMoveCommand = new DoComputerMoveCommand(this.computerSkillLevel);
        }

        public event Action OnCellPlayed;

        public ObservableCollection<HexCellViewModel> Cells
        {
            get { return this.cells; }
        }

        public int BoardSize { get; set; }

        public double ActualWidth { get; set; }
        
        public double ActualHeight { get; set; }

        public bool ShowDebugData { get; set; }

        public bool CanPlay { get; set; }

        public Board.HexBoard Board
        {
            get { return this.hexGame.Board; }
        }

        public HexGame Game
        {
            get { return this.hexGame; }
        }

        public Occupied NextPlayer
        {
            get
            {
                return this.hexGame.PlayerX.ToPlayer();
            }
        }

        public string SummaryText
        {
            get
            {
                return this.gameSummary.SummaryText;
            }
        }

        public string LastMoveDurationText
        {
            get
            {
                return this.gameSummary.LastMoveDurationText;
            }
        }

        public ICommand DoComputerMoveCommand
        {
            get { return this.doComputerMoveCommand; }
        }

        public ICommand GetDebugDataCommand
        {
            get { return this.getDebugDataCommand; }
        }

        public bool NextMoveIsHuman()
        {
            return this.gameSummary.NextMoveIsHuman();
        }

        public void CellWasPlayed(int x, int y)
        {
            this.hexGame.Play(x, y);
            this.hexGame.HasWon();

            this.OnPropertyChanged("SummaryText");

            this.CheckComputerMove();

            if (this.OnCellPlayed != null)
            {
                this.OnCellPlayed();
            }
        }

        public void Populate()
        {
            for (int x = 0; x < this.BoardSize; x++)
            {
                for (int y = 0; y < this.BoardSize; y++)
                {
                    HexCellViewModel hexCell = new HexCellViewModel(this)
                        {
                            BoardX = x,
                            BoardY = y
                        };

                    this.Cells.Add(hexCell);
                }
            }
        }

        public void SetActualBoardSize(Size newSize)
        {
            double cellSideDisplayLength = HexagonGeometry.CalculateSideLength(this.BoardSize, newSize.Width, newSize.Height);
            double verticalOffset = HexagonGeometry.CalculateVerticalOffset(this.BoardSize, newSize.Height, cellSideDisplayLength);
            double horizontalOffset = HexagonGeometry.CalculateHorizontalOffset(this.BoardSize, newSize.Width, cellSideDisplayLength);

            foreach (HexCellViewModel cell in this.Cells)
            {
                cell.VerticalOffset = verticalOffset;
                cell.HorizontalOffset = horizontalOffset;
                cell.CellSideDisplayLength = cellSideDisplayLength;
            }
        }

        public void ClearBoard()
        {
            this.hexGame = new HexGame(this.BoardSize);
            foreach (HexCellViewModel cell in this.Cells)
            {
                cell.Occupied = Occupied.Empty;
            }

            this.OnPropertyChanged("SummaryText");
        }

        public void SetLastMoveDuration(TimeSpan duration)
        {
            this.gameSummary.LastMoveDuration = duration;
            this.OnPropertyChanged("LastMoveDurationText");
        }

        public HexCellViewModel GetCellAtLocation(Location location)
        {
            foreach (HexCellViewModel cell in this.Cells)
            {
                if ((cell.BoardX == location.X) && (cell.BoardY == location.Y))
                {
                    return cell;
                }
            }

            return null;
        }

        private static int SkillLevelToDepth(ComputerSkillLevel skillLevel)
        {
            const int DefaultComputerSkillLevel = 4;

            switch (skillLevel)
            {
                case ComputerSkillLevel.Low:
                    return DefaultComputerSkillLevel - 1;

                case ComputerSkillLevel.Medium:
                    return DefaultComputerSkillLevel;

                case ComputerSkillLevel.Good:
                    return DefaultComputerSkillLevel + 1;

                case ComputerSkillLevel.Excellent:
                    return DefaultComputerSkillLevel + 2;

                default:
                    throw new Exception("Bad ComputerSkillLevel" + skillLevel);
            }
        }
        
        private void CopyCellStates(IEnumerable<HexCellViewModel> otherCells)
        {
            foreach (HexCellViewModel originalCell in otherCells)
            {
                HexCellViewModel newCell = this.GetCellAtLocation(originalCell.Location);
                newCell.Occupied = originalCell.Occupied;
            }
        }
        
        private void CheckComputerMove()
        {
            if (this.gameSummary.NextMoveIsComputer())
            {
                this.DoComputerMoveCommand.Execute(this);
            }
        }
    }
}
