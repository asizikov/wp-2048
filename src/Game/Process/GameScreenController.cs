using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Game.Lifecicle;
using Game.Resources;
using Game.Utils;
using GameEngine;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Game.Process
{
    internal class GameScreenController : IGameController
    {
        private readonly MainPage _view;
        private string _lastScore = string.Empty;
        private readonly BestScoresController _bestScoresController;

        private readonly Dictionary<int, int> _positions = new Dictionary<int, int>
        {
            {0, 0},
            {1, 113},
            {2, 226},
            {3, 339},
        };

        private ApplicationSettings _applicationSettings;

        public GameScreenController(MainPage view)
        {
            _applicationSettings = new ApplicationSettings();
            _view = view;
            _view.OverShare.Click += OverShareOnClick;
            _view.SettingsButton.Click += SettingsButtonOnClick;
            _view.LeaderboardButton.Click += LeaderboardButtonOnClick;
            _view.ShowGameField.Click += ShowGameFieldOnClick;
            
            _view.LayoutRoot.Background = _applicationSettings.Settings.BgColor.Color;
            StatisticsService.ReportGamePageLoaded();
            StatisticsService.ReportBgColor(_applicationSettings.Settings.BgColor);
            BuildApplicationBar();
            _bestScoresController = new BestScoresController();
        }

        private void ShowGameFieldOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            HideGameOverScreen();
        }

        private void LeaderboardButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            StatisticsService.PublishLeaderboardClicked();
            _view.NavigationService.Navigate(new Uri("/View/Leaderboard.xaml", UriKind.RelativeOrAbsolute));
        }

        private void SettingsButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            StatisticsService.PublishSettingsClicked();
            _view.NavigationService.Navigate(new Uri("/View/Settings.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OverShareOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            StatisticsService.PublishShareResultClick();
            if (_view != null)
            {
                _lastScore = _view.OverScore.Text;
            }

            var template = AppResources.GameShareTemplate;

            var shareLinkTask = new ShareLinkTask
            {
                Message = string.Format(template, _lastScore),
                LinkUri = new Uri("http://www.windowsphone.com/s?appid=66f975f4-d5fd-474c-be7e-81edae2361e4")

            };

            shareLinkTask.Show();
        }

        private void BuildApplicationBar()
        {
            _view.ApplicationBar = new ApplicationBar
            {
                Mode = ApplicationBarMode.Minimized,
                Opacity = 1.0,
                IsVisible = true,
                IsMenuEnabled = true
            };

            var aboutMenuItem = new ApplicationBarMenuItem {Text = AppResources.About};
            aboutMenuItem.Click += AboutButtonOnClick;
            _view.ApplicationBar.MenuItems.Add(aboutMenuItem);
        }

        private void AboutButtonOnClick(object sender, EventArgs eventArgs)
        {
            _view.NavigationService.Navigate(new Uri("/View/About.xaml", UriKind.RelativeOrAbsolute));
        }

        public void RedrawUi(GameGrid grid, GameStatus gameStatus)
        {

            UpdateScore(gameStatus.Score);
            _view.Field.Children.Clear();

            var cellsForMoveAnimation = new List<CellInfo>();

            for (int i = 0; i < grid.Cells.Length; i++)
            {
                for (int j = 0; j < grid.Cells.Length; j++)
                {
                    var cell = grid.Cells[i][j];
                    if (cell == null) continue;
                    var cellView = CellFactory.Create(cell);

                    if (cellView == null) continue;

                    if (cell.PreviousPosition != null)
                    {
                        cellsForMoveAnimation.Add(new CellInfo
                        {
                            Type = AnimationType.Move,
                            Cell = cell,
                            View = cellView,
                            PreviousePosition = cell.PreviousPosition
                        });
                        Canvas.SetLeft(cellView, _positions[cell.PreviousPosition.X]);
                        Canvas.SetTop(cellView, _positions[cell.PreviousPosition.Y]);
                    }
                    else if (cell.MergedFrom != null)
                    {
                        cellsForMoveAnimation.Add(new CellInfo
                        {
                            Type = AnimationType.MoveAndMerge,
                            Cell = cell,
                            View = cellView,
                            PreviousePosition = new Position
                            {
                                X = cell.MergedFrom[0].PreviousPosition.X,
                                Y = cell.MergedFrom[0].PreviousPosition.Y
                            }
                        });
                        Canvas.SetLeft(cellView, _positions[cell.MergedFrom[0].X]);
                        Canvas.SetTop(cellView, _positions[cell.MergedFrom[0].Y]);
                    }
                    else
                    {
                        cellsForMoveAnimation.Add(new CellInfo
                        {
                            Type = AnimationType.Appear,
                            Cell = cell,
                            View = cellView
                        });
                        Canvas.SetLeft(cellView, _positions[i]);
                        Canvas.SetTop(cellView, _positions[j]);
                    }

                    _view.Field.Children.Add(cellView);
                }
            }

            foreach (var pair in cellsForMoveAnimation)
            {
                AnimationFactory.ApplyAnimation(pair.Type, pair.View, pair.PreviousePosition, pair.Cell.X,
                    pair.Cell.Y);
            }
            SetGameOverStatus(gameStatus);
        }

        private void SetGameOverStatus(GameStatus gameStatus)
        {
            if ((gameStatus.Over || gameStatus.Won) && gameStatus.IsTermitated)
            {
                _bestScoresController.Persist();
                ShowGameOverScreen(gameStatus);
            }
            else
            {
                HideGameOverScreen();
            }
        }


        private void HideGameOverScreen()
        {
            if (_view.GameOver.Visibility == Visibility.Visible)
            {
                _view.GameOverBg.Visibility = Visibility.Collapsed;
                _view.GameOver.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowGameOverScreen(GameStatus gameStatus)
        {
            _view.GameOverBg.Visibility = Visibility.Visible;
            _view.GameOver.Visibility = Visibility.Visible;
            _view.GameOverFaidIn.Begin();
            _view.GameOverControalsSlideIn.Begin();
            

            _view.OverScore.Text = gameStatus.Score.ToString(CultureInfo.InvariantCulture);
            _view.OverStatus.Text = gameStatus.Won ? AppResources.GameYouWin : AppResources.GameGameOver;
            if (gameStatus.Won)
            {
                _view.OverKeepPlaying.Visibility = Visibility.Visible;
                _view.ShowGameField.Visibility = Visibility.Collapsed;
                StatisticsService.PublishWon();
            }
            else
            {
                _view.OverKeepPlaying.Visibility = Visibility.Collapsed;
                _view.ShowGameField.Visibility = Visibility.Visible;
            }
        }

        private void UpdateScore(int score)
        {
            _bestScoresController.SaveScore(score);
            var scoreStr = score.ToString(CultureInfo.InvariantCulture);
            var animateScore = scoreStr != _view.Score.Text;
            _view.Score.Text = scoreStr;
            if (animateScore)
            {
                _view.ScorePop.Begin();
            }
        }
    }
}