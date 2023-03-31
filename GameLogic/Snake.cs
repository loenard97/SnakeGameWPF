using System.Collections.Generic;
using System.Linq;

namespace SnakeGameWPF.GameLogic
{
    public enum MoveSnakeResult
    {
        Moved, Died, Ate
    };

    internal class Snake
    {
        private Directions _Direction = Directions.Right;
        public Directions Direction
        {
            get { return _Direction; }
            set
            {
                if (_Direction != value && _Direction != DirectionsExtension.Invert(value))
                    _Direction = value;
            }
        }
        public List<Vector2D> Body { get; set; } = new List<Vector2D> { new Vector2D { X = 10, Y = 10 }, new Vector2D { X = 10, Y = 9 }, new Vector2D { X = 10, Y = 8 } };

        public MoveSnakeResult MoveBody(Vector2D CurrentApple)
        {
            Vector2D newHead = new Vector2D { X = Body[0].X, Y = Body[0].Y };
            switch (Direction)
            {
                case Directions.Right:
                    newHead.Y = Body[0].Y + 1; break;
                case Directions.Left:
                    newHead.Y = Body[0].Y - 1; break;
                case Directions.Up:
                    newHead.X = Body[0].X - 1; break;
                case Directions.Down:
                    newHead.X = Body[0].X + 1; break;
            }

            if (newHead.X < 0 || newHead.Y < 0 || newHead.X > 29 || newHead.Y > 29)
                return MoveSnakeResult.Died;

            foreach (Vector2D BodySegment in Body)
            {
                if (newHead.X == BodySegment.X && newHead.Y == BodySegment.Y)
                {
                    return MoveSnakeResult.Died;
                }
            }

            MoveSnakeResult returnValue = MoveSnakeResult.Moved;
            for (int i = Body.Count - 1; i >= 0; --i)
            {
                if (i == 0)
                {
                    Body[i].X = newHead.X;
                    Body[i].Y = newHead.Y;
                    continue;
                }
                if (i == Body.Count - 1 && newHead.X == CurrentApple.X && newHead.Y == CurrentApple.Y)
                {
                    Body.Add(new Vector2D { X = Body.Last().X, Y = Body.Last().Y });
                    returnValue = MoveSnakeResult.Ate;
                }
                Body[i].X = Body[i - 1].X;
                Body[i].Y = Body[i - 1].Y;
            }
            return returnValue;
        }
    }
}
