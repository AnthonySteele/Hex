//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hex.Board;
    using Hex.Engine.Lookahead;
    using Hex.Engine.PathLength;

    /// <summary>
    /// the Hex game is the current game state
    /// data about the hex game in progress
    /// - board
    /// - Who plays next
    /// - has someone won
    /// - how many cells have been played 
    /// </summary>
    public class HexGame
    {
        // the main board
        private readonly HexBoard board;

        // game state - won yet?
        private readonly PathLengthBase xPathLength;
        private readonly PathLengthBase yPathLength;
        private readonly GoodMoves goodMoves;

        private bool currentPlayerX = true;
        private int countCellsPlayed;
        private Occupied winner;

        /// <summary>
        /// Initializes a new instance of the HexGame class, with a board size
        /// </summary>
        /// <param name="boardSize">size of the board</param>
        public HexGame(int boardSize)
        {
            this.board = new HexBoard(boardSize);
            IPathLengthFactory pathLengthFactory = new PathLengthAStarFactory();

            this.xPathLength = pathLengthFactory.CreatePathLength(this.board);
            this.yPathLength = pathLengthFactory.CreatePathLength(this.board);
            this.goodMoves = new GoodMoves();
            this.goodMoves.DefaultGoodMoves(boardSize, 5);
       }

        /// <summary>
        /// event to tell observers that a cell has been played as a computer move 
        /// </summary>
        public event EventHandler OnCellPlayed;
        
        public int BoardSize
        {
            get { return this.board.Size; }
        }

        public HexBoard Board
        {
            get { return this.board; }
        }

        public bool PlayerX 
        { 
            get { return this.currentPlayerX; } 
        }

        public int CountCellsPlayed 
        { 
            get { return this.countCellsPlayed; } 
        }

        public Occupied Winner
        {
            get { return this.winner; }
        }

        public GoodMoves GoodMoves
        {
            get { return this.goodMoves; }
        }

        /// <summary>
        /// The path length is the measure of who is winning
        /// if a player's path length is zero
        /// they have connected the opposite sides,
        /// thier opponent cannot do so (ininite path length)
        /// and they have won 
        /// </summary>
        /// <returns>the score for the current state of play</returns>
        public int SituationScore()
        {
            int playerXScore = this.xPathLength.PlayerScore(true);

            if (playerXScore == 0)
            {
                this.winner = Occupied.PlayerX;
                return this.board.Size;
            }
            
            if (playerXScore == PathLengthConstants.OffPath)
            {
                this.winner = Occupied.PlayerY;
                return -this.board.Size;
            }
            
            // compare the path lengths to see who is ahead
            int playerYScore = this.yPathLength.PlayerScore(false);
            this.winner = Occupied.Empty;

            return playerYScore - playerXScore;
        }

        public Occupied HasWon()
        {
            this.SituationScore();

            return this.winner;
        }

        public string DescribeHasWon()
        {
            switch (this.HasWon())
            {
                case Occupied.Empty:
                    return "No winner";

                case Occupied.PlayerX:
                    return "Player X wins";
                
                case Occupied.PlayerY:
                    return "Player Y wins";
                
                default:
                    return "Bad has-won value";
            }
        }

        public int PathValue(Location loc, bool playerX)
        {
            if (playerX)
            {
                return this.xPathLength.Value(loc);
            }

            return this.yPathLength.Value(loc);
        }

        public List<Location> GetCleanPath(bool playerX)
        {
            if (playerX)
            {
                return this.xPathLength.GetCleanPath(true);
            }

            return this.yPathLength.GetCleanPath(false);
        }

        /// <summary>
        /// Points that are on both paths
        /// </summary>
        /// <returns>the points that are on both paths</returns>
        public List<Location> GetCleanPathIntersection()
        {
            List<Location> xPath = this.GetCleanPath(true);
            List<Location> yPath = this.GetCleanPath(false);

            return xPath.Where(yPath.Contains).ToList(); 
        }

        public int PlayerScore(bool playerX)
        {
            if (playerX)
            {
                return this.xPathLength.PlayerScore(true);
            }

            return this.yPathLength.PlayerScore(false);
        }

        public void Play(int x, int y)
        {
            // play the cell
            this.board.PlayMove(x, y, this.currentPlayerX);

            // update stats
            this.currentPlayerX = !this.currentPlayerX;
            this.countCellsPlayed++;

            this.goodMoves.MoveUpRows();

            // notify
            if (this.OnCellPlayed != null)
            {
                this.OnCellPlayed(this, EventArgs.Empty);
            }
        }

        public void Clear()
        {
            this.board.Clear();
            this.currentPlayerX = true;
            this.countCellsPlayed = 0;
        }
    }
}
