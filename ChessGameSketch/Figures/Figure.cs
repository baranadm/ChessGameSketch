using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public abstract class Figure
    {
        public Vector2 position;
        public char sign;
        public readonly Player player;

        protected Figure(Vector2 position, Player player)
        {
            this.position = position;
            this.player = player;
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }

        public abstract PossibleMoves GetPossibleMoves();
    }

    public enum Player
    {
        White,
        Black
    }
}