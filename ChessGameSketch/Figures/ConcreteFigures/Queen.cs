using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Queen : Figure
    {
        public Queen(Vector2 position, Player player) : base(position, player)
        {
        }
        public override List<MoveDirection> GetFigureMoves()
        {
            return new List<MoveDirection>()
            {
                new MoveDirection(new Vector2(1, 1), true),
                new MoveDirection(new Vector2(1, 0), true),
                new MoveDirection(new Vector2(1, -1), true),
                new MoveDirection(new Vector2(0, -1), true),
                new MoveDirection(new Vector2(-1, -1), true),
                new MoveDirection(new Vector2(-1, 0), true),
                new MoveDirection(new Vector2(-1, 1), true),
                new MoveDirection(new Vector2(0, 1), true)
            };
        }
        public override FigureType FigureType()
        {
            return ChessGameSketch.FigureType.Queen;
        }
        public override Queen GetCopy()
        {
            return new Queen(new Vector2(Position.X, Position.Y), Player);
        }
    }
}