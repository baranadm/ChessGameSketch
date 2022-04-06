using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameSketch
{
    public class FigureMoves
    {
        private readonly List<Vector2> directions;
        private readonly bool repeatable;

        public FigureMoves(List<Vector2> directions, bool repeatable)
        {
            this.directions = directions;
            this.repeatable = repeatable;
        }

        public List<Vector2> Directions => directions;
        public bool Repeatable => repeatable;

        public override bool Equals(object? obj)
        {
            return obj is FigureMoves moves &&
                   EqualityComparer<List<Vector2>>.Default.Equals(directions, moves.directions) &&
                   repeatable == moves.repeatable;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(directions, repeatable);
        }
    }


}
