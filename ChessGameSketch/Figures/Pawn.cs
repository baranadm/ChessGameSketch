using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Pawn : Figure
    {
        public Pawn(Vector2 position, Player player) : base(position, player)
        {
        }

        public override FigureMoves GetFigureMoves()
        {
            List<Vector2> directions = new List<Vector2>();
            
            if(Player == Player.White)
            {
                directions.Add(new Vector2(0, 1));
                if(Position.Y==1)
                {
                    directions.Add(new Vector2(0, 2));
                }
            } else
            {
                directions.Add(new Vector2(0, -1));
                if(Position.Y==6)
                {
                    directions.Add(new Vector2(0, -2));
                }
            }

            return new FigureMoves(directions, false);
        }

        public FigureMoves GetAttackDirections()
        {
            List<Vector2> attackDirections = new List<Vector2>();
            if(Player == Player.White)
            {
                attackDirections.Add(new Vector2(1, 1));
                attackDirections.Add(new Vector2(-1, 1));
            } else
            {
                attackDirections.Add(new Vector2(1, -1));
                attackDirections.Add(new Vector2(-1, -1));
            }
            return new FigureMoves(attackDirections, false);
        }

        public override FigureType GetFigureType()
        {
            return FigureType.Pawn;
        }
        public override Pawn GetCopy()
        {
            return new Pawn(new Vector2(Position.X, Position.Y), Player);
        }
    }
}