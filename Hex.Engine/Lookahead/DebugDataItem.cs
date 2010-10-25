namespace Hex.Engine.Lookahead
{
    using Hex.Board;

    public class DebugDataItem
    {
        public int Lookahead { get; set; }

        public bool PlayerX { get; set; }

        public Location Location { get; set; }

        public int Alpha { get; set; }

        public int Beta { get; set; }

        public override string ToString()
        {
            string player = this.PlayerX ? "X" : "Y";
            return string.Format("Lookahead {0} Location {1} Player {2}", this.Lookahead, this.Location, player);
        }
    }
}
