using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Queen : Figure
    {
        public Queen(Vector2 position, Player player) : base(position, player)
        {
            Sign = '@';
        }
        public override PossibleMoves GetPossibleMoves()
        {
            return new PossibleMoves(new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(0, 1)
            },
            true);
        }
    }
}