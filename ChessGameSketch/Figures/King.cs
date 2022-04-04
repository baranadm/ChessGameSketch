using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    class King : Figure
    {
        public King(Vector2 position, Player player) : base(position, player)
        {
            sign = '#';
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
            false);
        }
    }
}