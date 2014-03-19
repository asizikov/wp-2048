using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Game.Lifecicle;
using Game.Resources;
using GameEngine;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Game.Process
{
    internal enum AnimationType
    {
        Appear,
        Move,
        MoveAndMerge
    }

    internal class Pair
    {
        public AnimationType Type { get; set; }
        public Border View { get; set; }
        public Tile Cell { get; set; }
        public Position PreviousePosition { get; set; }
    }

    internal class GameScreenController : IGameController
    {
        private MainPage _view;

        private readonly object _drawLock = new object();
        private string _lastScore = string.Empty;

        private readonly Dictionary<int, int> _positions = new Dictionary<int, int>
        {
            {0, 0},
            {1, 113},
            {2, 226},
            {3, 339},
        };

        public GameScreenController(MainPage view)
        {
            _view = view;
            _view.OverShare.Click += OverShareOnClick;
            _view.SettingsButton.Click += SettingsButtonOnClick;
            StatisticsService.ReportGamePageLoaded();
            _view.LayoutRoot.Background = new SolidColorBrush(CellFactory.ConvertStringToColor("#34aadc"));
            BuildApplicationBar();
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
            var shareStatusTask = new ShareStatusTask
            {
                Status = string.Format(template, _lastScore)
            };

            shareStatusTask.Show();
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
            UpdateScore(gameStatus.Score.ToString(CultureInfo.InvariantCulture));
            _view.Field.Children.Clear();

            var cellsForMoveAnimation = new List<Pair>();

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
                        cellsForMoveAnimation.Add(new Pair
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
                        cellsForMoveAnimation.Add(new Pair
                        {
                            Type = AnimationType.MoveAndMerge,
                            Cell = cell,
                            View = cellView,
                            PreviousePosition = new Position
                            {
                                X = cell.MergedFrom[0].X,
                                Y = cell.MergedFrom[0].Y
                            }
                        });
                        Canvas.SetLeft(cellView, _positions[cell.MergedFrom[0].X]);
                        Canvas.SetTop(cellView, _positions[cell.MergedFrom[0].Y]);
                    }
                    else
                    {
                        cellsForMoveAnimation.Add(new Pair
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
            ShowGameOverScreen(gameStatus);
            if (gameStatus.Over || gameStatus.Won)
            {
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
                StatisticsService.PublishWon();
            }
        }

        private void UpdateScore(string score)
        {
            var animateScore = score != _view.Score.Text;
            _view.Score.Text = score;
            if (animateScore)
            {
                _view.ScorePop.Begin();
            }
        }
    }
}