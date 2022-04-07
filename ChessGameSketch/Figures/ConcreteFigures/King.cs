using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class King : Figure
    {
        public King(Vector2 position, Player player) : base(position, player)
        {
        }

        public override List<MoveDirection> GetFigureMoves()
        {
            return new List<MoveDirection>()
            {
                new MoveDirection(new Vector2(1, 1), false),
                new MoveDirection(new Vector2(1, 0), false),
                new MoveDirection(new Vector2(1, -1), false),
                new MoveDirection(new Vector2(0, -1), false),
                new MoveDirection(new Vector2(-1, -1), false),
                new MoveDirection(new Vector2(-1, 0), false),
                new MoveDirection(new Vector2(-1, 1), false),
                new MoveDirection(new Vector2(0, 1), false)
            };
        }

        public override FigureType FigureType()
        {
            return ChessGameSketch.FigureType.King;
        }
        public override King GetCopy()
        {
            return new King(new Vector2(Position.X, Position.Y), Player);
        }
    }
}