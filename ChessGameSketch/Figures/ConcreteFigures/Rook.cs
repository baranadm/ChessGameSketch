using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Rook : Figure
    {
        public Rook(Vector2 position, Player player) : base(position, player)
        {
        }

        public override List<FigureMove> GetFigureMoves()
        {
            return new List<FigureMove>()
            {
                new FigureMove(new Vector2(1, 0), true),
                new FigureMove(new Vector2(0, -1), true),
                new FigureMove(new Vector2(-1, 0), true),
                new FigureMove(new Vector2(0, 1), true)
            };
        }
        public override FigureType GetFigureType()
        {
            return FigureType.Rook;
        }
        public override Rook GetCopy()
        {
            return new Rook(new Vector2(Position.X, Position.Y), Player);
        }
    }
}