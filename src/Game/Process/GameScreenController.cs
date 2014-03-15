using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Game.Lifecicle;
using GameEngine;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Game.Process
{
    internal class GameScreenController : IGameController, IDisposable
    {
        private MainPage _view;
        private int _size;
        private readonly object _drawLock = new object();

        public GameScreenController(MainPage view)
        {
            _view = view;
            _view.OverShare.Click += OverShareOnClick;
            StatisticsService.ReportGamePageLoaded();
            _view.LayoutRoot.Background = new SolidColorBrush(ConvertStringToColor("#34aadc"));
            BuildApplicationBar();
        }

        private void OverShareOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            StatisticsService.PublishShareResultClick();
            var shareStatusTask = new ShareStatusTask
            {
                Status = "I scored " + _view.OverScore.Text + " points at 2048 for Windows Phone, a game where you " +
                         "join numbers to score high! #2048game #wp2048"
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

            var aboutMenuItem = new ApplicationBarMenuItem {Text = "about"};
            aboutMenuItem.Click += AboutButtonOnClick;
            _view.ApplicationBar.MenuItems.Add(aboutMenuItem);
        }

        private void AboutButtonOnClick(object sender, EventArgs eventArgs)
        {
            _view.NavigationService.Navigate(new Uri("/View/About.xaml", UriKind.RelativeOrAbsolute));
        }

        public void RedrawUi(GameGrid grid, GameStatus gameStatus)
        {
            _view.Score.Text = gameStatus.Score.ToString(CultureInfo.InvariantCulture);
            lock (_drawLock)
            {
                _view.Field.Children.Clear();
                for (int i = 0; i < grid.Cells.Length; i++)
                {

                    var row = new StackPanel { Orientation = Orientation.Horizontal };
                    _view.Field.Children.Add(row);

                    for (int j = 0; j < grid.Cells.Length; j++)
                    {
                        var rect = CreateCell(grid, j, i);
                        row.Children.Add(rect);
                    }
                }
            }

            if (gameStatus.Over || gameStatus.Won)
            {
                _view.GameOverBg.Visibility = Visibility.Visible;
                _view.GameOver.Visibility = Visibility.Visible;

                _view.OverScore.Text = gameStatus.Score.ToString(CultureInfo.InvariantCulture);
                _view.OverStatus.Text = gameStatus.Won ? "You win!" : "Game over!";
                if (gameStatus.Won)
                {
                    StatisticsService.PublishWon();
                }
            }
            else
            {
                if (_view.GameOver.Visibility == Visibility.Visible)
                {
                    _view.GameOverBg.Visibility = Visibility.Collapsed;
                    _view.GameOver.Visibility = Visibility.Collapsed;
                }
            }
            
        }

        private Border CreateCell(GameGrid grid, int j, int i)
        {
            var cell = grid.Cells[j][i];
            var value = cell == null ? " " : cell.Value.ToString(CultureInfo.InvariantCulture);

            var cellTb = new TextBlock
            {
                Style = (Style) Application.Current.Resources["PhoneTextLargeStyle"],
                FontSize = GetFontSize(cell),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Gray),
                Text = value
            };

            _size = 90;
            var rect = new Border
            {
                Height = _size,
                Width = _size,
                Background = new SolidColorBrush(GetBackground(cell)),
                Margin = new Thickness(12, 12, 0, 0),
                Child = cellTb
            };
            return rect;
        }

        private static int GetFontSize(Tile cell)
        {
            if(cell == null || cell.Value < 128)
                return 57;
            if (cell.Value == 256) return 45;
            if (cell.Value < 1024) return 50;

            return 35;
        }

        private  Color GetBackground(Tile cell)
        {
            if (cell == null)
                return Colors.DarkGray;
            switch (cell.Value)
            {
                case 2:
                    return ConvertStringToColor("#eee4da");
                case 4:
                    return ConvertStringToColor("#ede0c8");
                case 8:
                    return ConvertStringToColor("#f2b179");
                case 16:
                    return ConvertStringToColor("#f59563");;
                case 32:
                    return ConvertStringToColor("#f67c5f");
                case 64:
                    return ConvertStringToColor("#f65e3b");
                case 128:
                    return ConvertStringToColor("#edcf72");
                case 256:
                    return ConvertStringToColor("#edc850");
                case 512:
                    return ConvertStringToColor("#edc53f");
                case 1024:
                    return ConvertStringToColor("#edc53f");
                case 2048:
                    return ConvertStringToColor("#edc22e");
                default:
                    return ConvertStringToColor("#eee4da");
            }
        }

        private static Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        public void Dispose()
        {
            _view = null;
        }
    }
}