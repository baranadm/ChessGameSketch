using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameSketch
{
    public class MoveDirection
    {
        public Vector2 Step { get; }
        public bool Repeatable { get; }

        public MoveDirection(Vector2 direction, bool repeatable)
        {
            this.Step = direction;
            this.Repeatable = repeatable;
        }
    }
}
