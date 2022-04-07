using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Pawn : Figure
    {
        public Pawn(Vector2 position, Player player) : base(position, player)
        {
        }

        public override List<MoveDirection> GetFigureMoves()
        {
            List<MoveDirection> moves = new List<MoveDirection>();
            
            if(Player == Player.White)
            {
                moves.Add(new MoveDirection(new Vector2(0, 1), false));
                if(Position.Y==1)
                {
                    moves.Add(new MoveDirection(new Vector2(0, 2), false));
                }
            } else
            {
                moves.Add(new MoveDirection(new Vector2(0, -1), false));
                if(Position.Y==6)
                {
                    moves.Add(new MoveDirection(new Vector2(0, -2), false));
                }
            }

            return moves;
        }

        public List<MoveDirection> GetAttackMoves()
        {
            List<MoveDirection> moves = new List<MoveDirection>();
            if(Player == Player.White)
            {
                moves.Add(new MoveDirection(new Vector2(1, 1), false));
                moves.Add(new MoveDirection(new Vector2(-1, 1), false));
            } else
            {
                moves.Add(new MoveDirection(new Vector2(1, -1), false));
                moves.Add(new MoveDirection(new Vector2(-1, -1), false));
            }
            return moves;
        }

        public override FigureType FigureType()
        {
            return ChessGameSketch.FigureType.Pawn;
        }
        public override Pawn GetCopy()
        {
            return new Pawn(new Vector2(Position.X, Position.Y), Player);
        }
    }
}