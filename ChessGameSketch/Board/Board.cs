using System.Numerics;

namespace ChessGameSketch
{
    public class Board
    {
        public List<Figure> FiguresOnBoard { get; set; }

        public Board()
        {
            FiguresOnBoard = new List<Figure>();
        }

        public Board(List<Figure> withFigures)
        {
            FiguresOnBoard = withFigures;
        }
        
        public void PutFigure(Figure newFigure)
        {
            FiguresOnBoard.Add(newFigure);
        }
        
        public void MoveFigure(Figure figure, Vector2 newPosition)
        {
            figure.Position = newPosition;
        }
        
        public void DeleteFigure(Figure figure)
        {
            FiguresOnBoard.Remove(figure);
        }
        public Figure? FigureOn(Vector2 position)
        {
            return FiguresOnBoard.Find(fig => fig.Position.Equals(position));
        }

        public List<Figure> FindAll(Player player)
        {
            return FiguresOnBoard.FindAll(fig => fig.Player == player);
        }

        public List<Figure> FindAll(FigureType type, Player player)
        {
            return FiguresOnBoard.FindAll(fig => fig.GetFigureType().Equals(type) && fig.Player == player);
        }

        public Board GetCopy()
        {
            return new Board(FiguresOnBoard.Select(fig => fig.GetCopy()).ToList());
        }
    }
}