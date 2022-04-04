using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Pawn : Figure
    {
        public Pawn(Vector2 position, Player player) : base(position, player)
        {
            sign = 'i';
        }

        public override PossibleMoves GetPossibleMoves()
        {
            List<Vector2> directions = new List<Vector2>();
            
            if(player == Player.White)
            {
                directions.Add(new Vector2(0, 1));
                if(position.Y==1)
                {
                    directions.Add(new Vector2(0, 2));
                }
            } else
            {
                directions.Add(new Vector2(0, -1));
                if(position.Y==6)
                {
                    directions.Add(new Vector2(0, -2));
                }
            }

            return new PossibleMoves(directions, false);
        }
    }
}