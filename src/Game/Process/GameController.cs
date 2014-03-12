using System;
using System.Globalization;
using System.Text;

namespace Game.Process {
    class GameController {
        private readonly MainWindow _view;

        public GameController(MainWindow view) {
            _view = view;
            
        }

        public void RedrawUi(GameGrid grid, GameStatus gameStatus) {
            _view.Score.Text = gameStatus.Score.ToString(CultureInfo.InvariantCulture);

            var sb = new StringBuilder();
            foreach (var row in grid.Cells) {
                foreach ( var cell in row) {
                    var value = cell == null ? "0" : cell.Value.ToString(CultureInfo.InvariantCulture);
                    sb.Append(value + "   |");
                }
                sb.Append(Environment.NewLine);
            }

            _view.Field.Text = sb.ToString();
        }
    }
}