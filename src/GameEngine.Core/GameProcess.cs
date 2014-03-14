using System;
using System.Collections.Generic;
using Game.Process;

namespace GameEngine
{
    public class GameProcess
    {
        private readonly IInputObserver _inputObserver;
        private readonly IGameController _gameController;
        private readonly int _size;
        private int _startTiles;
        private int _score;
        private bool _over;
        private bool _won;
        private GameGrid _grid;

        private readonly Dictionary<Direction, Position> _vectors = new Dictionary<Direction, Position>
        {
            {Direction.Up, new Position {X = 0, Y = -1}},
            {Direction.Right, new Position {X = 1, Y = 0}},
            {Direction.Down, new Position {X = 0, Y = 1}},
            {Direction.Left, new Position {X = -1, Y = 0}},
        };


        public GameProcess(IInputObserver inputObserver, IGameController gameController, int size)
        {
            if (inputObserver == null) throw new ArgumentNullException("inputObserver");
            if (gameController == null) throw new ArgumentNullException("gameController");
            _inputObserver = inputObserver;
            _gameController = gameController;
            _size = size;

            _inputObserver.Move += InputObserverOnMove;
            _inputObserver.Restart += InputObserverOnRestart;

            _startTiles = 2;

            Initialise();
        }

        public GameProcess(IInputObserver inputObserver, IGameController gameController, int size,
            GameState gameState)
        {
            if (inputObserver == null) throw new ArgumentNullException("inputObserver");
            if (gameController == null) throw new ArgumentNullException("gameController");
            _inputObserver = inputObserver;
            _gameController = gameController;
            _size = size;

            _inputObserver.Move += InputObserverOnMove;
            _inputObserver.Restart += InputObserverOnRestart;
            _startTiles = 2;
            Initialise(gameState);
        }


        public GameState GameStatusSnapshot
        {
            get
            {
                return new GameState
                {
                    Grid = _grid,
                    Score = _score,
                    Won = _won,
                    Over = _over
                };
            }
        }

        private void Initialise()
        {
            _grid = new GameGrid(_size);
            _score = 0;
            _over = false;
            _won = false;
            AddStartTiles();

            Actuate();
        }

        private void Initialise(GameState gameState)
        {
            _score = gameState.Score;
            _over = gameState.Over;
            _won = gameState.Won;
            _grid = gameState.Grid;
            Actuate();
        }

        private void AddStartTiles()
        {
            for (var i = 0; i < _startTiles; i++)
            {
                AddRandomTile();
            }
        }

        private void AddRandomTile()
        {
            if (_grid.CellsAvailable())
            {
                var rnd = new Random();

                var value = rnd.NextDouble() < 0.9 ? 2 : 4;
                var tile = new Tile(_grid.RandomAvailableCell(), value);

                _grid.InsertTile(tile);
            }
        }

        private void InputObserverOnRestart()
        {
            Initialise();
        }

        private void InputObserverOnMove(Direction direction)
        {
            if (_over || _won) return; // Don't do anything if the game's over

            var vector = GetVector(direction);
            var traversals = BuildTraversals(vector);
            var moved = false;

            PrepareTiles();

            foreach (var x in traversals.X)
            {
                foreach (var y in traversals.Y)
                {
                    var cell = new Position {X = x, Y = y};
                    var tile = _grid.CellContent(cell);

                    if (tile != null)
                    {
                        var positions = FindFarthestPosition(cell, vector);
                        var next = _grid.CellContent(positions.Next);

                        // Only one merger per row traversal?
                        if (next != null && next.Value == tile.Value && next.MergedFrom == null)
                        {
                            var merged = new Tile(positions.Next, tile.Value*2);
                            merged.MergedFrom = new[] {tile, next};

                            _grid.InsertTile(merged);
                            _grid.RemoveTile(tile);

                            // Converge the two tiles' positions
                            tile.UpdatePosition(positions.Next);

                            // Update the score
                            _score += merged.Value;

                            // The mighty 2048 tile
                            if (merged.Value == 2048) _won = true;
                        }
                        else
                        {
                            MoveTile(tile, positions.Farthest);
                        }

                        if (!PositionsEqual(cell, tile))
                        {
                            moved = true; // The tile moved from its original cell!
                        }
                    }
                }
            }

            if (moved)
            {
                AddRandomTile();

                if (!MovesAvailable())
                {
                    _over = true;
                }

                Actuate();
            }
        }

        private void Actuate()
        {
            _gameController.RedrawUi(_grid, new GameStatus
            {
                Score = _score,
                Over = _over,
                Won = _won
            });
        }

        private bool MovesAvailable()
        {
            return _grid.CellsAvailable() || TileMatchesAvailable();
        }

        private bool TileMatchesAvailable()
        {
            Tile tile;

            for (var x = 0; x < _size; x++)
            {
                for (var y = 0; y < _size; y++)
                {
                    tile = _grid.CellContent(new Position {X = x, Y = y});

                    if (tile != null)
                    {
                        for (var direction = 0; direction < 4; direction++)
                        {
                            var vector = GetVector((Direction) direction);
                            var cell = new Position {X = x + vector.X, Y = y + vector.Y};

                            var other = _grid.CellContent(cell);

                            if (other != null && other.Value == tile.Value)
                            {
                                return true; // These two tiles can be merged
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool PositionsEqual(Position first, Tile second)
        {
            return first.X == second.X && first.Y == second.Y;
        }

        private void MoveTile(Tile tile, Position cell)
        {
            _grid.Cells[tile.X][tile.Y] = null;
            _grid.Cells[cell.X][cell.Y] = tile;
            tile.UpdatePosition(cell);
        }

        private FarthestPosition FindFarthestPosition(Position cell, Position vector)
        {
            Position previous;

            // Progress towards the vector direction until an obstacle is found
            do
            {
                previous = cell;
                cell = new Position {X = previous.X + vector.X, Y = previous.Y + vector.Y};
            } while (_grid.WithinBounds(cell) &&
                     _grid.CellAvailable(cell));

            return new FarthestPosition
            {
                Farthest = previous,
                Next = cell
            };
        }

        private void PrepareTiles()
        {
            _grid.EachCell((x, y, tile) =>
            {
                if (tile != null)
                {
                    tile.MergedFrom = null;
                    tile.SavePosition();
                }
            });
        }

        private Traversals BuildTraversals(Position vector)
        {
            var traversals = new Traversals
            {
                X = new List<int>(),
                Y = new List<int>()
            };

            for (var pos = 0; pos < _size; pos++)
            {
                traversals.X.Add(pos);
                traversals.Y.Add(pos);
            }

            // Always traverse from the farthest cell in the chosen direction
            if (vector.X == 1) traversals.X.Reverse();
            if (vector.Y == 1) traversals.Y.Reverse();

            return traversals;
        }

        private Position GetVector(Direction direction)
        {
            return _vectors[direction];
        }
    }

    public class GameStatus
    {
        public int Score { get; set; }
        public bool Over { get; set; }
        public bool Won { get; set; }
    }

    internal class FarthestPosition
    {
        public Position Farthest { get; set; }
        public Position Next { get; set; }
    }

    internal class Traversals
    {
        public List<int> X { get; set; }
        public List<int> Y { get; set; }
    }
}