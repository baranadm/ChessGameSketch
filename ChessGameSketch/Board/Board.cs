using System.Numerics;

namespace ChessGameSketch
{
    public class Board
    {
        public List<Figure> FiguresAtBoard { get; set; }

        public Board()
        {
            FiguresAtBoard = new List<Figure>();
        }
        
        public void PutFigure(Figure newFigure)
        {
            FiguresAtBoard.Add(newFigure);
        }
        
        public void MoveFigure(Figure figure, Vector2 newPosition)
        {
            figure.Position.X = newPosition.X;
            figure.Position.Y = newPosition.Y;
        }
        
        public void DeleteFigure(Figure figure)
        {
            FiguresAtBoard.Remove(figure);
        }
    }
}