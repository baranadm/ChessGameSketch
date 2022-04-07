using System.Numerics;

namespace ChessGameSketch
{
    public class Bishop : Figure
    {
        public Bishop(Vector2 position, Player player) : base(position, player)
        {
        }

        public override List<MoveDirection> GetFigureMoves()
        {
            return new List<MoveDirection>()
            {
                new MoveDirection(new Vector2(1, 1),true),
                new MoveDirection(new Vector2(-1,1),true),
                new MoveDirection(new Vector2(1,-1),true),
                new MoveDirection(new Vector2(-1,-1), true)
            };
        }

        public override FigureType FigureType()
        {
            return ChessGameSketch.FigureType.Bishop;
        }
        public override Bishop GetCopy()
        {
            return new Bishop(new Vector2(Position.X, Position.Y), Player);
        }

    }

}