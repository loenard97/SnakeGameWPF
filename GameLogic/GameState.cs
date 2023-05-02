using System;

namespace SnakeGameWPF.GameLogic
{
    public enum GameStateReturn
    {
        Updated, GameOver,
    }

    public enum Directions
    {
        Up, Down, Left, Right,
    };

    public static class DirectionsExtension
    {
        public static Directions Invert(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    return Directions.Down;
                case Directions.Down:
                    return Directions.Up;
                case Directions.Left:
                    return Directions.Right;
                case Directions.Right:
                    return Directions.Left;
                default:
                    throw new InvalidCastException();
            }
        }
    }

    internal class GameState
    {
        public int Speed { get; set; } = 3;
        public int Score { get; set; } = 0;
        public bool GameOver { get; set; } = false;
        public Snake Snake { get; }
        public Vector2D Apple { get; set; }

        public GameState()
        {
            Snake = new Snake();
            SpawnNewApple();
        }

        public GameStateReturn Update()
        {
            switch (Snake.MoveBody(Apple))
            {
                case MoveSnakeResult.Moved:
                    return GameStateReturn.Updated;
                case MoveSnakeResult.Died:
                    GameOver = true;
                    return GameStateReturn.GameOver;
                case MoveSnakeResult.Ate:
                    Score += 1;
                    SpawnNewApple();
                    return GameStateReturn.Updated;
            }
            return GameStateReturn.GameOver;
        }

        private void SpawnNewApple()
        {
            Random rand = new Random();
            Apple = new Vector2D { X = rand.Next(0, 30), Y = rand.Next(0, 30) };
        }
    }
}
