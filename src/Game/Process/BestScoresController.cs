using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;
using Game.Utils;

namespace Game.Process
{
    internal class BestScoresController
    {
        private readonly List<int> _bestScores;
        private int _current;
        private bool _bestScoreReported;
        private readonly ApplicationSettings _applicationSettings;

        public BestScoresController()
        {
            _applicationSettings = new ApplicationSettings();
            var scores = _applicationSettings.Settings.BestScores;
               
            scores.Sort((a, b) => -1*a.CompareTo(b));
            _bestScores = scores;
        }

        public void SaveScore(int score)
        {
            _current = score;
            if (_bestScores.Any() && _current > _bestScores.First() && !_bestScoreReported)
            {
                _bestScoreReported = true;
                var toast = new ToastPrompt
                {
                    Title = Resources.AppResources.BestScoreToast,
                    TextWrapping = TextWrapping.Wrap,
                    Background = new SolidColorBrush(Color.FromArgb(255,0,191,255)),
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
//                    ImageSource = new BitmapImage(new Uri("/Resources/Images/notification_logo.png", UriKind.Relative))
                };

                toast.Show();
            }
        }

        public void Persist()
        {
            if (_bestScores.Count == 0)
            {
                _bestScores.Add(_current);
                _applicationSettings.Settings.BestScores = _bestScores;
                _applicationSettings.SaveSettings();
            }
            if ((_current > _bestScores.Last() || _bestScores.Count < 5) && !_bestScores.Contains(_current))
            {
                _bestScores.Add(_current);
               var toStore =  _bestScores.OrderByDescending(x => x).Take(5).ToList();
                _applicationSettings.Settings.BestScores = toStore;
                _applicationSettings.SaveSettings();
            }
            _current = 0;
        }

        public List<int> Scores()
        {
          return _applicationSettings.Settings.BestScores;
        }
    }
}