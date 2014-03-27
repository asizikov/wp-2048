using System;
using System.Windows;
using System.Windows.Input;
using Game.Process;

namespace Game.Input
{
    public class SwipeInputObserver : BaseInputObserver
    {
        private readonly MainPage _view;

        public SwipeInputObserver(MainPage view)
        {
            _view = view;
            _view.LayoutRoot.ManipulationCompleted += LayoutRootOnManipulationDelta;
            _view.ResetButton.Click += ResetButtonOnClick;
            _view.OverReset.Click += ResetButtonOnClick;
            _view.OverKeepPlaying.Click += OverKeepPlayingOnClick;
        }

        private void OverKeepPlayingOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FireKeepPlaying();
        }

        private void ResetButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            RestartWhithConfirmation();
        }

        private void LayoutRootOnManipulationDelta(object sender, ManipulationCompletedEventArgs e)
        {
            var x = e.TotalManipulation.Translation.X;
            var y = e.TotalManipulation.Translation.Y;

            if (Math.Abs(x) < 25 && Math.Abs(y) < 25) return;

            if (Math.Abs(x) > Math.Abs(y))
            {
                FireMove(x >= 0 ? Direction.Right : Direction.Left);
            }
            else
            {
                FireMove(y >= 0 ? Direction.Down : Direction.Up);
            }
        }

        public override void Dispose()
        {
            if (_view == null) return;
            _view.LayoutRoot.ManipulationCompleted -= LayoutRootOnManipulationDelta;
            _view.ResetButton.Click -= ResetButtonOnClick;
            _view.OverReset.Click -= ResetButtonOnClick;
            _view.OverKeepPlaying.Click -= OverKeepPlayingOnClick;
        }
    }
}