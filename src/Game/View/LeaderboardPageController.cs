using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Game.Process;

namespace Game.View
{
    internal class BestScoreItem
    {
        public string Number { get; set; }
        public string Value { get; set; }
    }

    public class LeaderboardPageController
    {
        private readonly Leaderboard _view;
        private BestScoresController _bestScoresController;
        private ObservableCollection<BestScoreItem> _bestScoreItems;

        public LeaderboardPageController(Leaderboard view)
        {
            _view = view;
            _bestScoresController = new BestScoresController();
            _bestScoreItems = new ObservableCollection<BestScoreItem>();


            var number = 0;
            var  best = _bestScoresController.Scores();
            if (best.Count == 0)
            {
                _view.Scores.Visibility = Visibility.Collapsed;
                _view.BestScoreEmpty.Visibility = Visibility.Visible;
            }
            else
            {
                _view.Scores.Visibility = Visibility.Visible;
                _view.BestScoreEmpty.Visibility = Visibility.Collapsed;

                foreach (var score in best)
                {
                    number++;
                    _bestScoreItems.Add(new BestScoreItem
                    {
                        Number = "#" + number.ToString(CultureInfo.InvariantCulture),
                        Value = score.ToString(CultureInfo.InvariantCulture)
                    });
                }

                _view.Scores.ItemsSource = _bestScoreItems;
            }
        }
    }
}