using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Rook : Figure
    {
        public Rook(Vector2 position, Player player) : base(position, player)
        {
        }

        public override List<MoveDirection> GetFigureMoves()
        {
            return new List<MoveDirection>()
            {
                new MoveDirection(new Vector2(1, 0), true),
                new MoveDirection(new Vector2(0, -1), true),
                new MoveDirection(new Vector2(-1, 0), true),
                new MoveDirection(new Vector2(0, 1), true)
            };
        }
        public override FigureType FigureType()
        {
            return ChessGameSketch.FigureType.Rook;
        }
        public override Rook GetCopy()
        {
            return new Rook(new Vector2(Position.X, Position.Y), Player);
        }
    }
}