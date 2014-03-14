using System.Windows.Navigation;
using Game.Process;
using Game.Utils;
using GameEngine;

namespace Game
{
    public partial class MainPage
    {
        private InputObserver _inputObserver;
        private GameScreenController _gameScreenController;
        private GameProcess _gameProcess;
        private readonly ApplicationSettings _applicationSettings;

        public MainPage()
        {
            InitializeComponent();
            _applicationSettings = new ApplicationSettings();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _inputObserver = new InputObserver(UpButton, DownButton, LeftButton, RightButton, ResetButton, OverReset);
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
            _gameScreenController.Dispose();
        }
    }
}