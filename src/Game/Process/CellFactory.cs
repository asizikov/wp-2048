using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game.Process
{
    internal static class CellFactory
    {
        private const int CellSize = 100;


        public static Border Create(Tile cell)
        {
            if (cell == null) return null;
            var value = cell.Value.ToString(CultureInfo.InvariantCulture);

            var cellTb = new TextBlock
            {
                Style = (Style)Application.Current.Resources["PhoneTextLargeStyle"],
                FontSize = GetFontSize(cell),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Gray),
                Text = value
            };
            var rect = new Border
            {
                Height = CellSize,
                Width = CellSize,
                Background = new SolidColorBrush(GetBackground(cell)),
                Child = cellTb
            };

            return rect;
        }

        private static int GetFontSize(Tile cell)
        {
            if (cell == null || cell.Value < 128)
                return 57;
            if (cell.Value == 256) return 45;
            if (cell.Value < 1024) return 50;

            return 35;
        }

        public static Color GetBackground(Tile cell)
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
                    return ConvertStringToColor("#f59563");
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

        public static Color ConvertStringToColor(String hex)
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
                a = Byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = Byte.Parse(hex.Substring(start, 2), NumberStyles.HexNumber);
            g = Byte.Parse(hex.Substring(start + 2, 2), NumberStyles.HexNumber);
            b = Byte.Parse(hex.Substring(start + 4, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}