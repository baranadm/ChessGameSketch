using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Knight : Figure
    {
        public Knight(Vector2 position, Player player) : base(position, player)
        {
            sign = 'S';
        }

        public override PossibleMoves GetPossibleMoves()
        {
            return new PossibleMoves(new List<Vector2>()
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
    }
}