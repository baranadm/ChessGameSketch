using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Knight : Figure
    {
        public Knight(Vector2 position, Player player) : base(position, player)
        {
        }

        public override List<FigureMove> GetFigureMoves()
        {
            return new List<FigureMove>()
            {
                new FigureMove(new Vector2(-2, 1), false),
                new FigureMove(new Vector2(-2, -1), false),
                new FigureMove(new Vector2(2, 1), false),
                new FigureMove(new Vector2(2, -1), false),
                new FigureMove(new Vector2(1, 2), false),
                new FigureMove(new Vector2(-1,2), false),
                new FigureMove(new Vector2(1,-2), false),
                new FigureMove(new Vector2(-1,-2), false)
            };
        }
        public override FigureType FigureType()
        {
            return ChessGameSketch.FigureType.Knight;
        }
        public override Knight GetCopy()
        {
            return new Knight(new Vector2(Position.X, Position.Y), Player);
        }
    }
}