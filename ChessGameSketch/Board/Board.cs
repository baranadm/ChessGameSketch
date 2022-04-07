using ChessGameSketch.Exceptions;
using System.Numerics;

namespace ChessGameSketch
{
    public class Board
    {
        public Vector2? EnPassantField { get; set; }
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
            if (newFigure.Position.X < 0 || newFigure.Position.X > 7 || newFigure.Position.Y < 0 || newFigure.Position.Y > 7)
                throw new OutOfBoundsException("Figure's position is out of bounds.");
            if (FigureOn(newFigure.Position) != null) throw new FieldOccupiedException($"Field {newFigure.Position} is occupied another figure");
            FiguresOnBoard.Add(newFigure);
        }
        
        public void MoveFigure(Figure figure, Vector2 newPosition)
        {
            Figure? chosen = FindFigure(figure);
            if (chosen == null) throw new FigureNotFoundException();
            if(FigureOn(newPosition) != null && FigureOn(newPosition).SamePlayerAs(chosen))
                throw new FieldOccupiedException($"Field {newPosition} is occupied by friendly figure");

            HandleEnPassantField(figure, newPosition);

            figure.Position = newPosition;
        }

        public void MoveFigure(Vector2 actualPosition, Vector2 newPosition)
        {
            MoveFigure(FigureOn(actualPosition), newPosition);
        }

        public void DeleteFigure(Figure figure)
        {
            if (FindFigure(figure) == null) throw new FigureNotFoundException($"Not found: {figure}");
            FiguresOnBoard.Remove(figure);
        }

        public Figure? FigureOn(Vector2 position)
        {
            return FiguresOnBoard.Find(fig => fig.Position.Equals(position));
        }
        
        private Figure? FindFigure(Figure figure)
        {
            return FiguresOnBoard.Find(f => f.Equals(figure));
        }

        public List<Figure> FindPlayersFigures(Player player)
        {
            return FiguresOnBoard.FindAll(fig => fig.Player == player);
        }

        public List<Figure> FindPlayersFiguresWithType(FigureType type, Player player)
        {
            return FiguresOnBoard.FindAll(fig => fig.FigureType().Equals(type) && fig.Player == player);
        }

        private void HandleEnPassantField(Figure figure, Vector2 newPosition)
        {
            ClearEnPassantField();
            MarkEnPassantIfRequired(figure, newPosition);
        }

        private void ClearEnPassantField()
        {
            EnPassantField = null;
        }

        private void MarkEnPassantIfRequired(Figure figure, Vector2 newPosition)
        {
            if(figure.FigureType() == FigureType.Pawn)
                if(Vector2.Distance(figure.Position, newPosition) == 2)
                    EnPassantField = Vector2.Divide(Vector2.Add(figure.Position, newPosition), new Vector2(0, 2));
        }

        public Board GetCopy()
        {
            return new Board(FiguresOnBoard.Select(fig => fig.GetCopy()).ToList());
        }
    }
}