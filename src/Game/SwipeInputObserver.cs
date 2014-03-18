using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Game.Process;
using GameEngine;

namespace Game
{
    public class SwipeInputObserver : IInputObserver
    {
        private readonly MainPage _view;

        public SwipeInputObserver(MainPage view)
        {
            _view = view;
            _view.LayoutRoot.ManipulationCompleted += LayoutRootOnManipulationDelta;
            _view.ResetButton.Click += ResetButtonOnClick;
            _view.OverReset.Click += ResetButtonOnClick;
        }

        private void ResetButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Restart != null)
            {
                Restart();
            }
        }

        private void LayoutRootOnManipulationDelta(object sender, ManipulationCompletedEventArgs e)
        {
            var x = e.TotalManipulation.Translation.X;
            var y = e.TotalManipulation.Translation.Y;

            if(Math.Abs(x) < 25 && Math.Abs(y) < 25) return;

            if (Math.Abs(x) > Math.Abs(y))
            {
                FireMove(x >= 0 ? Direction.Right : Direction.Left);
            }
            else
            {
                FireMove(y >= 0? Direction.Down : Direction.Up);
            }
        }

        private void FireMove(Direction direction)
        {
            if (Move != null)
            {
                Move(direction);
            }
        }

        public event Action<Direction> Move;
        public event Action Restart;

        public void Dispose()
        {
            if (_view == null) return;
            _view.LayoutRoot.ManipulationCompleted -= LayoutRootOnManipulationDelta;
            _view.ResetButton.Click -= ResetButtonOnClick;
            _view.OverReset.Click -= ResetButtonOnClick;
        }
    }
}