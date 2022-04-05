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

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return $"{this.GetType().Name}, {player}, {position}, {sign}";
        }
    }

    public enum Player
    {
        White,
        Black
    }
}