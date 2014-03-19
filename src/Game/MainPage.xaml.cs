using System.Windows;
using System.Windows.Navigation;
using Game.Process;
using Game.Utils;
using GameEngine;

namespace Game
{
    public partial class MainPage
    {
        private IInputObserver _inputObserver;
        private GameScreenController _gameScreenController;
        private GameProcess _gameProcess;
        private ApplicationSettings _applicationSettings;

        public MainPage()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _applicationSettings = new ApplicationSettings();
            if (_applicationSettings.Settings.UseSwipe)
            {
                _inputObserver = new SwipeInputObserver(this);
                SwipeControl.Visibility = Visibility.Visible;
                ButtonsControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                _inputObserver = new InputObserver(UpButton, DownButton, LeftButton, RightButton, ResetButton, OverReset);
                SwipeControl.Visibility = Visibility.Collapsed;
                ButtonsControl.Visibility = Visibility.Visible;
            }
            
            _gameScreenController = new GameScreenController(this);
            _gameProcess = CreateGameProcess();
        }

        private GameProcess CreateGameProcess()
        {
            if (_applicationSettings.HasStoredGame)
            {
                return new GameProcess(_inputObserver, _gameScreenController, 4, _applicationSettings.LoadGameState());
            }

            return new GameProcess(_inputObserver, _gameScreenController, 4);
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _applicationSettings.Save(_gameProcess);
            base.OnNavigatedFrom(e);
            _inputObserver.Dispose();
        }
    }
}