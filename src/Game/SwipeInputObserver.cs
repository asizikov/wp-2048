using System;
using System.Windows;
using System.Windows.Input;
using Game.Process;
using GameEngine;

namespace Game
{
    public class SwipeInputObserver : IInputObserver
    {
        private static readonly int Delta = 50;
        private readonly MainPage _view;

        public SwipeInputObserver(MainPage view)
        {
            _view = view;
            _view.LayoutRoot.ManipulationDelta += LayoutRootOnManipulationDelta;
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

        private void LayoutRootOnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {

            if (e.CumulativeManipulation.Translation.X > Delta &&
                (e.CumulativeManipulation.Translation.Y > -25 &&
                 e.CumulativeManipulation.Translation.Y < 25))
            {
                FireMove(Direction.Right);
            }
            if (e.CumulativeManipulation.Translation.X < -Delta &&
                (e.CumulativeManipulation.Translation.Y > -25 &&
                 e.CumulativeManipulation.Translation.Y < 25))
            {
                FireMove(Direction.Left);
            }
            if (e.CumulativeManipulation.Translation.Y > Delta &&
               (e.CumulativeManipulation.Translation.X > -25 &&
                e.CumulativeManipulation.Translation.X < 25))
            {
                FireMove(Direction.Down);
            }
            if (e.CumulativeManipulation.Translation.Y < -Delta &&
                (e.CumulativeManipulation.Translation.X > -25 &&
                 e.CumulativeManipulation.Translation.X < 25))
            {
                FireMove(Direction.Up);
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
            if(_view == null) return;
            _view.LayoutRoot.ManipulationDelta -= LayoutRootOnManipulationDelta;
            _view.ResetButton.Click -= ResetButtonOnClick;
            _view.OverReset.Click -= ResetButtonOnClick;
        }
    }
}