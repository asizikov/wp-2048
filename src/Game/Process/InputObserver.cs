using System;
using System.Windows;
using System.Windows.Controls;
using GameEngine;

namespace Game.Process
{
    internal class InputObserver : IInputObserver
    {
        private Button _upButton;
        private Button _downButton;
        private Button _leftButton;
        private Button _rightButton;
        private Button _resetButton;
        private Button _overReset;
        public event Action Restart;
        public event Action<Direction> Move;

        public InputObserver(Button upButton, Button downButton, Button leftButton, Button rightButton,
            Button resetButton, Button overReset)
        {
            _upButton = upButton;
            _downButton = downButton;
            _leftButton = leftButton;
            _rightButton = rightButton;
            _resetButton = resetButton;
            _overReset = overReset;
            _upButton.Click += UpButtonOnClick;
            _overReset.Click += ResetButtonOnClick;
            _downButton.Click += DownButtonOnClick;
            _rightButton.Click += RightButtonOnClick;
            _leftButton.Click += LeftButtonOnClick;
            _resetButton.Click += ResetButtonOnClick;
        }

        private void ResetButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FireReset();
        }

        private void LeftButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FireMove(Direction.Left);
        }

        private void RightButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FireMove(Direction.Right);
        }

        private void DownButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FireMove(Direction.Down);
        }

        private void UpButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FireMove(Direction.Up);
        }


        private void FireReset()
        {
            if (Restart != null) Restart();
        }


        private void FireMove(Direction direction)
        {
            if (Move != null) Move(direction);
        }

        public void Dispose()
        {
            _downButton.Click -= DownButtonOnClick;
            _upButton.Click -= UpButtonOnClick;
            _leftButton.Click -= LeftButtonOnClick;
            _rightButton.Click -= RightButtonOnClick;
            _resetButton.Click -= ResetButtonOnClick;
            _overReset.Click -= ResetButtonOnClick;
            _downButton = null;
            _upButton = null;
            _leftButton = null;
            _rightButton = null;
            _resetButton = null;
            _overReset = null;
        }
    }
}