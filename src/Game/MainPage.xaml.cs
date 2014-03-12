using System.Windows.Navigation;
using Game.Process;
using GameEngine;

namespace Game
{
    public partial class MainPage
    {
        private InputObserver _inputObserver;
        private GameScreenController _gameScreenController;
        private GameProcess _gameProcess;

        public MainPage()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _inputObserver = new InputObserver(UpButton, DownButton, LeftButton, RightButton, ResetButton, OverReset);
            _gameScreenController = new GameScreenController(this);
            _gameProcess = new GameProcess(_inputObserver, _gameScreenController, 4);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _inputObserver.Dispose();
            _gameScreenController.Dispose();
        }
    }
}