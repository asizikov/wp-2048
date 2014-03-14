using Game.Process;

namespace GameEngine
{

    public  class GameState
    {
        public GameGrid Grid { get; set; }
        public int Score { get; set; }
        public bool Over { get; set; }
        public bool Won { get; set; }
    }
}
