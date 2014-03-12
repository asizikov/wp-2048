using System;
using System.Windows.Controls;

namespace Game.Process {
    internal class InputObserver {
        public event Action Restart;
        public event Action<Direction> Move;

        public InputObserver(Button upButton, Button downButton, Button leftButton, Button rightButton,
            Button resetButton) {
            upButton.Click += (sender, args) => FireMove(Direction.Up);
            downButton.Click += (sender, args) => FireMove(Direction.Down);
            rightButton.Click += (sender, args) => FireMove(Direction.Right);
            leftButton.Click += (sender, args) => FireMove(Direction.Left);
            resetButton.Click += (sender, args) => FireReset();
        }


        private void FireReset() {
            if (Restart != null) Restart();
        }


        private void FireMove(Direction direction) {
            if (Move != null) Move(direction);
        }
    }
}