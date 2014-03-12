using System;
using System.Collections.Generic;

namespace Game.Process {
    public class Position {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Tile {
        public Tile(Position position, int value) {
            X = position.X;
            Y = position.Y;
            Value = value;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }
        public Tile[] MergedFrom { get; set; }
        private Position PreviousPosition { get; set; }

        public void SavePosition() {
            PreviousPosition = new Position {X = X, Y = Y};
            PreviousPosition = new Position {X = X, Y = Y};
        }

        public void UpdatePosition(Position position) {
            X = position.X;
            Y = position.Y;
        }
    }

    internal class GameGrid {
        public Tile[][] Cells { get; set; }
        private readonly int _size;

        public GameGrid(int size) {
            _size = size;
            Cells = new Tile[size][];
            Build();
        }

        

        private void Build() {
            for (var x = 0; x < _size; x++) {
                Cells[x] = new Tile[_size];
                for (var y = 0; y < _size; y++) {
                    Cells[x][y] = null;
                }
            }
        }

        public Position RandomAvailableCell() {
            var cells = AvailableCells();

            if (cells.Count != 0) {
                var rand = new Random();
                return cells[rand.Next(cells.Count)];
            }
            return null;
        }

        private List<Position> AvailableCells() {
            var cells = new List<Position>();

            EachCell((x, y, tile) => {
                if (tile == null) {
                    cells.Add(new Position {X = x, Y = y});
                }
            });

            return cells;
        }

        public bool WithinBounds(Position position) {
            return position.X >= 0 && position.X < _size &&
                   position.Y >= 0 && position.Y < _size;
        }

        public void RemoveTile(Tile tile) {
            Cells[tile.X][tile.Y] = null;
        }

        public void InsertTile(Tile tile) {
            Cells[tile.X][tile.Y] = tile;
        }


        public Tile CellContent(Position cell) {
            if (WithinBounds(cell)) {
                return Cells[cell.X][cell.Y];
            }
            return null;
        }

        private bool CellOccupied(Position cell) {
            return CellContent(cell) != null;
        }


        public bool CellAvailable(Position cell) {
            return !CellOccupied(cell);
        }

        public bool CellsAvailable() {
            return AvailableCells().Count > 0;
        }

        public void EachCell(Action<int, int, Tile> callback) {
            for (var x = 0; x < _size; x++) {
                for (var y = 0; y < _size; y++) {
                    callback(x, y, Cells[x][y]);
                }
            }
        }
    }
}