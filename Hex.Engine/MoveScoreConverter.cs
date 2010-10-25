namespace Hex.Engine
{
    using System;
    using Hex.Board;

    /// <summary>
    /// Problem: Convert a move score, which is complex, into a single nubmer 
    /// so that it can be stored, compared and negated efficiently
    /// Short paths are better, so basic number difference in pathlengths
    /// A win is a path length of zero, so a win is = 1000
    /// Win in less moves is better, so add (10 - no moves) to wins
    /// Multiply by -1 for player y (minimizing player)
    /// </summary>
    public static class MoveScoreConverter
    {
        private const int WinScore = 10000;
        private const int ScoreMultiplier = 100;
        private const int MaxDepth = 20;

        public static int ConvertWin(Occupied winner, int depth)
        {
            int result = (WinScore + MaxDepth) - depth;
            return NegateForPlayerY(result, winner);
        }

        public static int ConvertScore(int score, int depth)
        {
            return (score * ScoreMultiplier) + (MaxDepth - depth);
        }

        public static bool IsWin(int score)
        {
            return Math.Abs(score) >= WinScore;
        }

        public static Occupied Winner(int score)
        {
            if (IsWinForPlayer(score, Occupied.PlayerX))
            {
                return Occupied.PlayerX;
            }

            if (IsWinForPlayer(score, Occupied.PlayerY))
            {
                return Occupied.PlayerY;
            }

            return Occupied.Empty;
        }

        public static bool IsWinForPlayer(int score, Occupied player)
        {
            int positveScore = NegateForPlayerY(score, player);
            return positveScore >= WinScore;
        }

        public static bool IsBetterFor(int score1, int score2, bool playerX)
        {
            if (playerX)
            {
                return score1 > score2;
            }

            return score1 < score2;
        }

        public static bool IsImmediateWin(Occupied player, int score)
        {
            if (!IsWinForPlayer(score, player))
            {
                return false;
            }

            if (player == Occupied.PlayerX)
            {
                return WinDepth(score) == 0;
            }
            
            if (player == Occupied.PlayerY)
            {
                return WinDepth(score) == 0;                                
            }

            return false;
        }

        public static string DescribeScore(int score)
        {
            if (IsWin(score))
            {
                return string.Format("Win by {0} in {1} moves", DescribeWinner(score), WinDepth(score));
            }

            if (score == 0)
            {
                return string.Format("Score: {0}", score);                
            }

            return string.Format("{0} ahead with {1}", DescribeWinner(score), score);
        }

        public static int WinDepth(int score)
        {
            return (WinScore + MaxDepth) - Math.Abs(score);
        }

        private static string DescribeWinner(int score)
        {
            return (score > 0) ? "X" : "Y";
        }

        private static int NegateForPlayerY(int score, Occupied player)
        {
            if (player == Occupied.PlayerY)
            {
                return score * -1;
            }

            return score;
        }
    }
}
