using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Bishop : Figure
    {
        public Bishop(Vector2 position, Player player) : base(position, player)
        {
            sign = 'A';
        }

        public override PossibleMoves GetPossibleMoves()
        {
            return new PossibleMoves(new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(-1,1),
                new Vector2(1,-1),
                new Vector2(-1,-1)
            }, 
            true);
        }
    }

}