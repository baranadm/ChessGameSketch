using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class King : Figure
    {
        public King(Vector2 position, Player player) : base(position, player)
        {
        }

        public override FigureMoves GetFigureMoves()
        {
            return new FigureMoves(new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(0, 1)
            },
            false);
        }

        public override FigureType GetFigureType()
        {
            return FigureType.King;
        }
        public override King GetCopy()
        {
            return new King(new Vector2(Position.X, Position.Y), Player);
        }
    }
}