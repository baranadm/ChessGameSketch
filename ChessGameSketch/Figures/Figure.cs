using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public abstract class Figure
    {
        public Vector2 Position { get; set; }
        public Player Player { get; }

        protected Figure(Vector2 position, Player player)
        {
            this.Position = position;
            this.Player = player;
        }

        public abstract List<MoveDirection> GetFigureMoves();
        public abstract FigureType FigureType();
        public abstract Figure GetCopy();
        public void UpdatePosition(Vector2 newPosition)
        {
            this.Position = newPosition;
        }

        public bool SamePlayerAs(Figure other)
        {
            return other.Player.Equals(Player);
        }

        public bool IsType(FigureType type)
        {
            return FigureType().Equals(type);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return $"{this.GetType().Name}, {Player}, {Position}";
        }

    }
}