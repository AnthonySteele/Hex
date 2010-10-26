//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Media;

    using Hex.Board;
    using Hex.Wpf.Helpers;

    public class HexCellViewModel : BaseViewModel
    {
        private readonly HexBoardViewModel board;
        private readonly HexCellDebugDataViewModel debugData = new HexCellDebugDataViewModel();

        private int boardX;
        private int boardY;

        private double canvasX;
        private double canvasY;
        private double displayWidth;
        private double displayHeight;

        private double cellSideDisplayLength;
        private Visibility bottomLineVisible;
        private Visibility bottomRightLineVisible;
        private Visibility topRightLineVisible;
        private Brush bottomLineBrush;
        private Brush topLineBrush;
        private Brush bottomLeftLineBrush;
        private Brush bottomRightLineBrush;
        private Brush topLeftLineBrush;
        private Brush topRightLineBrush;

        private Occupied occupied;
        private Occupied previousOccupied;
        private Brush playerBrush;
        private bool mouseIsOver;

        public HexCellViewModel(HexBoardViewModel board)
        {
            this.board = board;
            this.PlayerBrush = this.CalculatePlayerBrush();
        }

        /// <summary>
        /// Gets or sets the Cell's X position on the baord
        /// </summary>
        public int BoardX
        {
            get
            {
                return this.boardX;
            }

            set
            {
                this.boardX = value;
                this.PositionCell();
            }
        }

        /// <summary>
        /// Gets or sets the Cell's Y position on the baord
        /// </summary>
        public int BoardY
        {
            get
            {
                return this.boardY;
            }

            set
            {
                this.boardY = value;
                this.PositionCell();
            }
        }

        public Location Location
        {
            get
            {
                return new Location(this.BoardX, this.BoardY);
            }
        }

        /// <summary>
        /// Gets the number of cells across the board
        /// </summary>
        public int BoardSize
        {
            get
            {
                return this.board.BoardSize;
            }
        }

        /// <summary>
        /// Gets or sets the size of the hex cell when displayed
        /// </summary>
        public double CellSideDisplayLength
        {
            get
            {
                return this.cellSideDisplayLength;
            }

            set
            {
                this.cellSideDisplayLength = value;
                this.PositionCell();
            }
        }

        /// <summary>
        /// Gets the cell's X position on the board canvas
        /// </summary>
        public double CanvasX 
        {
            get
            {
                return this.canvasX;
            }  

            private set
            {
                if (this.canvasX != value)
                {
                    this.canvasX = value;
                    this.OnPropertyChanged("CanvasX");
                }
            }
        }

        /// <summary>
        /// Gets the cell's Y position on the board canvas
        /// </summary>
        public double CanvasY
        {
            get
            {
                return this.canvasY;
            }

            private set
            {
                if (this.canvasY != value)
                {
                    this.canvasY = value;
                    this.OnPropertyChanged("CanvasY");
                }
            }
        }

        public double DisplayWidth
        {
            get
            {
                return this.displayWidth;
            }

            private set
            {
                if (this.displayWidth != value)
                {
                    this.displayWidth = value;
                    this.OnPropertyChanged("DisplayWidth");
                }
            }
        }

        public double DisplayHeight
        {
            get
            {
                return this.displayHeight;
            }

            private set
            {
                if (this.displayHeight != value)
                {
                    this.displayHeight = value;
                    this.OnPropertyChanged("DisplayHeight");
                }
            }
        }

        public Visibility BottomLineVisible
        {
            get
            {
                return this.bottomLineVisible;
            }

            set
            {
                if (this.bottomLineVisible != value)
                {
                    this.bottomLineVisible = value;
                    this.OnPropertyChanged("BottomLineVisible");
                }
            }
        }

        public Visibility BottomRightLineVisible
        {
            get
            {
                return this.bottomRightLineVisible;
            }

            set
            {
                if (this.bottomRightLineVisible != value)
                {
                    this.bottomRightLineVisible = value;
                    this.OnPropertyChanged("BottomRightLineVisible");
                }
            }
        }

        public Visibility TopRightLineVisible
        {
            get
            {
                return this.topRightLineVisible;
            }

            set
            {
                if (this.topRightLineVisible != value)
                {
                    this.topRightLineVisible = value;
                    this.OnPropertyChanged("TopRightLineVisible");
                }
            }
        }

        public Brush BottomLineBrush
        {
            get
            {
                return this.bottomLineBrush;
            }

            set
            {
                if (this.bottomLineBrush != value)
                {
                    this.bottomLineBrush = value;
                    this.OnPropertyChanged("BottomLineBrush");
                }
            }
        }

        public Brush TopLineBrush
        {
            get
            {
                return this.topLineBrush;
            }

            set
            {
                if (this.topLineBrush != value)
                {
                    this.topLineBrush = value;
                    this.OnPropertyChanged("TopLineBrush");
                }
            }
        }

        public Brush BottomLeftLineBrush
        {
            get
            {
                return this.bottomLeftLineBrush;
            }

            set
            {
                if (this.bottomLeftLineBrush != value)
                {
                    this.bottomLeftLineBrush = value;
                    this.OnPropertyChanged("BottomLeftLineBrush");
                }
            }
        }

        public Brush BottomRightLineBrush
        {
            get
            {
                return this.bottomRightLineBrush;
            }

            set
            {
                if (this.bottomRightLineBrush != value)
                {
                    this.bottomRightLineBrush = value;
                    this.OnPropertyChanged("BottomRightLineBrush");
                }
            }
        }

        public Brush TopLeftLineBrush
        {
            get
            {
                return this.topLeftLineBrush;
            }

            set
            {
                if (this.topLeftLineBrush != value)
                {
                    this.topLeftLineBrush = value;
                    this.OnPropertyChanged("TopLeftLineBrush");
                }
            }
        }
        
        public Brush TopRightLineBrush
        {
            get
            {
                return this.topRightLineBrush;
            }

            set
            {
                if (this.topRightLineBrush != value)
                {
                    this.topRightLineBrush = value;
                    this.OnPropertyChanged("TopRightLineBrush");
                }
            }
        }

        public Brush PlayerBrush
        {
            get
            {
                return this.playerBrush;
            }

            set
            {
                if (this.playerBrush != value)
                {
                    this.playerBrush = value;
                    this.OnPropertyChanged("PlayerBrush");
                }
            }
        }

        public double VerticalOffset { get; set; }

        public double HorizontalOffset { get; set; }

        public Occupied Occupied
        {
            get
            {
                return this.occupied;
            }

            set
            {
                if (this.occupied != value)
                {
                    this.previousOccupied = this.occupied;
                    this.occupied = value;
                    this.PlayerBrush = this.CalculatePlayerBrush();
                    this.OnPropertyChanged("Occupied");
                }
            }
        }

        public string DebugLocation
        {
            get { return this.Location.ToString(); }
        }

        public Visibility DebugDataVisible
        {
            get { return this.board.ShowDebugData.ToVisibilityCollapsed(); }
        }

        public HexCellDebugDataViewModel DebugData
        {
            get { return this.debugData; }
        }

        public bool CanPlay
        {
            get { return this.board.CanPlay; }
        }
        
        public bool NextMoveIsHuman()
        {
            return this.board.NextMoveIsHuman();
        }
        
        public void PlayCell()
        {
            this.Occupied = this.board.NextPlayer;
            this.mouseIsOver = false;
            this.board.CellWasPlayed(this.boardX, this.boardY);
        }

        public override string ToString()
        {
            return this.BoardX + ", " + this.BoardY + " at " + this.CanvasX + "," + this.CanvasY;
        }

        public void EnterCell()
        {
            this.mouseIsOver = true;
            this.PlayerBrush = this.CalculatePlayerBrush();
        }

        public void LeaveCell()
        {
            this.mouseIsOver = false;
        }

        private static Brush PlayerXBrush()
        {
            return ApplicationBrush("PlayerXBrush");
        }

        private static Brush PlayerYBrush()
        {
            return ApplicationBrush("PlayerYBrush");
        }

        private static Brush ApplicationBrush(string brushName)
        {
            return (Brush)Application.Current.MainWindow.FindResource(brushName);
        }

        private Brush CalculatePlayerBrush()
        {
            if (this.Occupied == Occupied.PlayerX)
            {
                return PlayerXBrush();
            }
            
            if (this.Occupied == Occupied.PlayerY)
            {
                return PlayerYBrush();
            }

            if (this.mouseIsOver)
            {
                return this.BrushForNextPlayer();
            }

            return this.BrushForFadeOut();
        }

        private Brush CalculateBottomLineBrush()
        {
            if ((this.BoardY == 0) && (this.BoardX != this.board.BoardSize - 1))
            {
                return PlayerXBrush();
            }

            if ((this.BoardX == this.board.BoardSize - 1) && (this.BoardY != 0))
            {
                return PlayerYBrush();
            }

            return Brushes.Black;
        }

        private Brush CalculateTopLeftLineBrush()
        {
            if (this.BoardX == 0)
            {
                return PlayerYBrush();
            }

            return Brushes.Black;
        }

        private Brush CalculateBottomLeftLineBrush()
        {
            if (this.BoardY == 0)
            {
                return PlayerXBrush();
            }

            return Brushes.Black;
        }

        private Brush CalculateBottomRightLineBrush()
        {
            if (this.BoardX == (this.board.BoardSize - 1))
            {
                return PlayerYBrush();
            }

            return Brushes.Black;
        }
        
        private Brush CalculateTopLineBrush()
        {
            if (this.BoardY == (this.board.BoardSize - 1) && (this.BoardX != 0))
            {
                return PlayerXBrush();
            }

            if ((this.BoardX == 0) && this.BoardY != (this.board.BoardSize - 1))
            {
                return PlayerYBrush();
            } 
            
            return Brushes.Black;
        }

        private Brush CalculateTopRightLineBrush()
        {
            if (this.BoardY == (this.board.BoardSize - 1))
            {
                return PlayerXBrush();
            }

            return Brushes.Black;
        }
        
        private Brush BrushForFadeOut()
        {
            if (this.previousOccupied == Occupied.PlayerX)
            {
                return PlayerXBrush();
            }

            if (this.previousOccupied == Occupied.PlayerY)
            {
                return PlayerYBrush();
            }

            return null;
        }

        private Brush BrushForNextPlayer()
        {
            if (this.board.NextPlayer == Occupied.PlayerX)
            {
                return PlayerXBrush();
            }

            if (this.board.NextPlayer == Occupied.PlayerY)
            {
                return PlayerYBrush();
            }

            return null;
        }

        private void PositionCell()
        {
            if (this.BoardSize == 0)
            {
                return;
            }

            int ups = (this.BoardX - this.BoardY) + this.BoardSize - 1;
            double across = this.BoardX + this.BoardY;

            this.DisplayWidth = HexagonGeometry.CellWidth(this.CellSideDisplayLength);
            this.DisplayHeight = HexagonGeometry.CellHeight(this.CellSideDisplayLength);

            this.CanvasY = this.VerticalOffset + (ups * this.DisplayHeight / 2);
            this.CanvasX = this.HorizontalOffset + (across * this.DisplayWidth / 2);

            bool lastRowX = this.BoardX == this.BoardSize - 1;
            bool lastRowY = this.BoardY == this.BoardSize - 1;

            this.BottomRightLineVisible = lastRowX.ToVisibility();
            this.BottomLineVisible = (this.BoardY == 0 || lastRowX).ToVisibility();
            this.TopRightLineVisible = lastRowY.ToVisibility();
            this.BottomLineBrush = this.CalculateBottomLineBrush();
            this.TopLineBrush = this.CalculateTopLineBrush();
            this.TopLeftLineBrush = this.CalculateTopLeftLineBrush();
            this.BottomLeftLineBrush = this.CalculateBottomLeftLineBrush();
            this.BottomRightLineBrush = this.CalculateBottomRightLineBrush();
            this.TopRightLineBrush = this.CalculateTopRightLineBrush();
        }
    }
}
