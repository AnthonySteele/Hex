namespace Hex.Engine.PathLength
{
    using System.Collections.Generic;
    using Hex.Board;

    /// <summary>
    /// Data on how all locations were scored
    /// </summary>
    public class CleanPathData : Dictionary<Location, CleanPathDataItem>
    {
        private bool playerX;
        private int boardSize;

        public void SetScore(Location dest, int score, List<Location> sources)
        {
            if (this.ContainsKey(dest))
            {
                // do we have a lower score?
                CleanPathDataItem existingItem = this[dest];
                if (existingItem.Score > score)
                {
                    existingItem.Score = score;
                    existingItem.SourceLocations = sources;
                }
                else if (existingItem.Score == score)
                {
                    // same score, so this is also a potential source
                    existingItem.SourceLocations.AddRange(sources);
                }
            }
            else
            {
                // add it
                CleanPathDataItem newItem = new CleanPathDataItem
                    {
                        Score = score, SourceLocations = sources
                    };

                Add(dest, newItem);
            }
        }

        public void SetScore(Location dest, int score, params Location[] sources)
        {
            List<Location> sourcesList = new List<Location>(sources);
            this.SetScore(dest, score, sourcesList);
        }

        public void SetScore(Location dest, int score)
        {
            List<Location> sources = new List<Location>();
            this.SetScore(dest, score, sources);
        }

        public void Clean(bool cleanPlayerX, int cleanBoardSize)
        {
            this.playerX = cleanPlayerX;
            this.boardSize = cleanBoardSize;

            // keep only the lowest in the final row
            this.CleanFinalRow();

            // throw out all that do not have a following cell
            int removedCount;
            do
            {
                removedCount = this.CleanPathStep();
            }
            while (removedCount > 0);
        }

        private static bool ItemIsInFinalRow(bool itemPlayerX, int boardSize, Location loc)
        {
            bool itemIsInFinalRow;
            if (itemPlayerX)
            {
                itemIsInFinalRow = loc.Y == (boardSize - 1);
            }
            else
            {
                itemIsInFinalRow = loc.X == (boardSize - 1);
            }

            return itemIsInFinalRow;
        }
        
        private void CleanFinalRow()
        {
            // find the lowest score in the final row
            int lowestScore = int.MaxValue;
            foreach (Location loc in this.Keys)
            {
                if (ItemIsInFinalRow(this.playerX, this.boardSize, loc))
                {
                    int itemScore = this[loc].Score;
                    if (itemScore < lowestScore)
                    {
                        lowestScore = itemScore;
                    }
                }
            }

            // remove the rest in the final row
            foreach (Location loc in this.Keys)
            {
                if (ItemIsInFinalRow(this.playerX, this.boardSize, loc))
                {
                    int itemScore = this[loc].Score;
                    if (itemScore > lowestScore)
                    {
                        this[loc].Valid = false;
                    }
                }
            }

            this.RemoveInvalid();
        }

        private int CleanPathStep()
        {
            List<Location> invalidLocations = new List<Location>();
            foreach (Location loc in this.Keys)
            {
                if (!this.HasFollower(loc))
                {
                    invalidLocations.Add(loc);
                }
            }

            // remove them
            int removeCount = invalidLocations.Count;
            foreach (Location loc in invalidLocations)
            {
                this.Remove(loc);
            }

            // return the count of items removed
            return removeCount;
        }

        private bool HasFollower(Location loc)
        {
            if (ItemIsInFinalRow(this.playerX, this.boardSize, loc))
            {
                return true;
            }

            foreach (var value in this.Values)
            {
                if (value.HasSourceLocation(loc))
                {
                    return true;
                }
            }

            return false;
        }

        private void RemoveInvalid()
        {
            // find invalid locations
            List<Location> invalidLocations = new List<Location>();
            foreach (Location loc in this.Keys)
            {
                if (! this[loc].Valid)
                {
                    invalidLocations.Add(loc);
                }
            }

            foreach (Location loc in invalidLocations)
            {
                this.Remove(loc);
            }
        }
    }
}
