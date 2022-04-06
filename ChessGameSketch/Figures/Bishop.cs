using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Bishop : Figure
    {
        public Bishop(Vector2 position, Player player) : base(position, player)
        {
        }

        public override FigureMoves GetFigureMoves()
        {
            return new FigureMoves(new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(-1,1),
                new Vector2(1,-1),
                new Vector2(-1,-1)
            }, 
            true);
        }

        public override FigureType GetFigureType()
        {
            return FigureType.Bishop;
        }
        public override Bishop GetCopy()
        {
            return new Bishop(new Vector2(Position.X, Position.Y), Player);
        }


    }

}