using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public abstract class Figure
    {
        public Vector2 Position { get; set; }
        public char Sign { get; set; }
        public Player Player { get; }

        protected Figure(Vector2 position, Player player)
        {
            this.Position = position;
            this.Player = player;
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            this.Position = newPosition;
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
            return $"{this.GetType().Name}, {Player}, {Position}, {Sign}";
        }

        public bool SamePlayerAs(Figure other)
        {
            return other.Player.Equals(Player);
        }
    }

    public enum Player
    {
        White,
        Black
    }
}