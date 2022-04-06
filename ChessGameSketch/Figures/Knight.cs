using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Knight : Figure
    {
        public Knight(Vector2 position, Player player) : base(position, player)
        {
        }

        public override FigureMoves GetFigureMoves()
        {
            return new FigureMoves(new List<Vector2>()
            {
                new Vector2(-2, 1),
                new Vector2(-2, -1),
                new Vector2(2, 1),
                new Vector2(2, -1),
                new Vector2(1, 2),
                new Vector2(-1,2),
                new Vector2(1,-2),
                new Vector2(-1,-2)
            },
            false);
        }
        public override FigureType GetFigureType()
        {
            return FigureType.Knight;
        }
        public override Knight GetCopy()
        {
            return new Knight(new Vector2(Position.X, Position.Y), Player);
        }
    }
}