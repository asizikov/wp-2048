using System.Windows;
using Game.Process;

namespace Game {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private InputObserver _inputObserver;
        private GameController _gameController;
        private GameProcess _gameProcess;

        public MainWindow() {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            _inputObserver = new InputObserver(UpButton,DownButton,LeftButton, RightButton, ResetButton);
            _gameController = new GameController(this);
            _gameProcess = new GameProcess(_inputObserver, _gameController, 4);
        }
    }
}
