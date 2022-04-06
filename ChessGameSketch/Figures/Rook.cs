﻿using ChessGameSketch;
using System.Numerics;

namespace ChessGameSketch
{
    public class Rook : Figure
    {
        public Rook(Vector2 position, Player player) : base(position, player)
        {
        }

        public override FigureMoves GetFigureMoves()
        {
            return new FigureMoves(new List<Vector2>()
            {
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(-1, 0),
                new Vector2(0, 1)
            },
            true);
        }
        public override FigureType GetFigureType()
        {
            return FigureType.Rook;
        }
        public override Rook GetCopy()
        {
            return new Rook(new Vector2(Position.X, Position.Y), Player);
        }
    }
}