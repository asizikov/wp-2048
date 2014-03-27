using System.Windows;
using Game.Process;

namespace Game.Input
{
    public class InputObserver : BaseInputObserver
    {
        private readonly MainPage _view;

        public InputObserver(MainPage view)
        {
            _view = view;
            _view.UpButton.Click += UpButtonOnClick;
            _view.OverReset.Click += ResetButtonOnClick;
            _view.DownButton.Click += DownButtonOnClick;
            _view.RightButton.Click += RightButtonOnClick;
            _view.LeftButton.Click += LeftButtonOnClick;
            _view.ResetButton.Click += ResetButtonOnClick;
            _view.OverKeepPlaying.Click += OverKeepPlayingOnClick;
        }


        private void OverKeepPlayingOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FireKeepPlaying();
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


        public override void Dispose()
        {
            if (_view == null) return;
            _view.DownButton.Click -= DownButtonOnClick;
            _view.UpButton.Click -= UpButtonOnClick;
            _view.LeftButton.Click -= LeftButtonOnClick;
            _view.RightButton.Click -= RightButtonOnClick;
            _view.ResetButton.Click -= ResetButtonOnClick;
            _view.OverReset.Click -= ResetButtonOnClick;
            _view.OverKeepPlaying.Click -= OverKeepPlayingOnClick;
        }
    }
}