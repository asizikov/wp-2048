using System;
using Game.Process;

namespace GameEngine
{
    public interface IInputObserver
    {
        event Action<Direction> Move;
        event Action Restart;
    }
}