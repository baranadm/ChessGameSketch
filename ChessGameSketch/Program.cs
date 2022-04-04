using System;
using System.Numerics;

namespace ChessGameSketch
{
    public class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.Show();

            board.PutFigure(new Pawn(new Vector2(0, 1), Player.White));
            board.PutFigure(new Pawn(new Vector2(1, 1), Player.White));
            board.PutFigure(new Pawn(new Vector2(2, 1), Player.White));
            board.PutFigure(new Pawn(new Vector2(3, 1), Player.White));
            board.PutFigure(new Pawn(new Vector2(4, 1), Player.White));
            board.PutFigure(new Pawn(new Vector2(5, 1), Player.White));
            board.PutFigure(new Pawn(new Vector2(6, 1), Player.White));
            board.PutFigure(new Pawn(new Vector2(7, 1), Player.White));
            board.PutFigure(new Rook(new Vector2(0, 0), Player.White));
            board.PutFigure(new Rook(new Vector2(7, 0), Player.White));
            board.PutFigure(new Bishop(new Vector2(2, 0), Player.White));
            board.PutFigure(new Bishop(new Vector2(5, 0), Player.White));
            board.PutFigure(new Knight(new Vector2(1, 0), Player.White));
            board.PutFigure(new Knight(new Vector2(6, 0), Player.White));
            board.PutFigure(new King(new Vector2(4, 0), Player.White));
            board.PutFigure(new Queen(new Vector2(3, 0), Player.White));
            board.Show();
        }

    }
}