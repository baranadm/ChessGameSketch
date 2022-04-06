using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameSketch
{
    public class FigureMove
    {
        public Vector2 Direction { get; }
        public bool Repeatable { get; }

        public FigureMove(Vector2 direction, bool repeatable)
        {
            this.Direction = direction;
            this.Repeatable = repeatable;
        }

    }

}
