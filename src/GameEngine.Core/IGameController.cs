using Game.Process;

namespace GameEngine
{
    public interface IGameController
    {
        void RedrawUi(GameGrid grid, GameStatus gameStatus);
    }
}