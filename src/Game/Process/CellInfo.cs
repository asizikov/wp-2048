using System.Windows.Controls;

namespace Game.Process
{
    internal class CellInfo
    {
        public AnimationType Type { get; set; }
        public Border View { get; set; }
        public Tile Cell { get; set; }
        public Position PreviousePosition { get; set; }
    }
}