using System.Numerics;

namespace ChessGameSketch
{
    public class Bishop : Figure
    {
        public Bishop(Vector2 position, Player player) : base(position, player)
        {
        }

        public override List<FigureMove> GetFigureMoves()
        {
            return new List<FigureMove>()
            {
                new FigureMove(new Vector2(1, 1),true),
                new FigureMove(new Vector2(-1,1),true),
                new FigureMove(new Vector2(1,-1),true),
                new FigureMove(new Vector2(-1,-1), true)
            };
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