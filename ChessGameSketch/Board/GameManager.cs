using System.Numerics;

namespace ChessGameSketch
{
    public class GameManager
    {
        private Board Board { get; set; }

        public GameManager(Board board)
        {
            this.Board = board;
        }

        public List<Vector2> GetAllowedMoves(List<Figure> allFiguresAtBoard, Figure figureInspected)
        {
            PossibleMoves possibleMoves = figureInspected.GetPossibleMoves();

            List<Vector2> allowedMoves = new List<Vector2>();
            for (int i = 0; i < possibleMoves.Directions.Count; i++)
            {
                Vector2 actualPosition = figureInspected.Position;
                Vector2 actualDirection = possibleMoves.Directions[i];

                while (true)
                {
                    Vector2 nextPositionInActualDirection = Vector2.Add(actualPosition, actualDirection);

                    bool outOfBounds = nextPositionInActualDirection.X < 0 || nextPositionInActualDirection.X > 7 || nextPositionInActualDirection.Y < 0 || nextPositionInActualDirection.Y > 7;
                    if (outOfBounds) break;

                    //break if next tile is occupied by another figure with same color (player)
                    Figure? figureOnNextPosition = FigureAt(nextPositionInActualDirection);
                    bool samePlayerOnNextPosition = ( figureOnNextPosition == null ) ? false: figureOnNextPosition.SamePlayerAs(figureInspected);
                    if (samePlayerOnNextPosition) break;

                    // if there is king of figure's player on the board, check, if it will not be checked after taking a move
                    King? king = (King) allFiguresAtBoard.Find(fig => fig.GetType().Name.Equals("King") && fig.Player.Equals(figureInspected.Player));
                    if (king != null)
                    {
                        //Board.MoveFigure(figure, nextPosition);
                        figureInspected.UpdatePosition(nextPositionInActualDirection);
                        Boolean isKingChecked = IsChecked(king);
                        //Board.MoveFigure(figure, actualPosition);
                        figureInspected.UpdatePosition(actualPosition);
                        if (isKingChecked) break;
                        if (!isKingChecked) allowedMoves.Add(nextPositionInActualDirection);
                    }

                    // if next tile is occupied by another figure of another color (different player), add this tile to possible moves (taking a figure) and break
                    if (samePlayerOnNextPosition != null && !samePlayerOnNextPosition.Player.Equals(figureInspected.Player))
                    {
                        allowedMoves.Add(nextPositionInActualDirection);
                        break;
                    }

                    // if next positions tile is free, then add next tile to possible moves and proceed to to next tile
                    if (FigureAt(nextPositionInActualDirection) == null)
                    {
                        allowedMoves.Add(nextPositionInActualDirection);
                        actualPosition.X = nextPositionInActualDirection.X;
                        actualPosition.Y = nextPositionInActualDirection.Y;
                    }

                    // break if move is not repetable
                    if (!possibleMoves.Repeatable) break;
                }

            }
            return allowedMoves;
        }

        //TODO this method should be private
        public bool IsChecked(King king)
        {
            List<Figure> figuresWithoutKing = Board.FiguresAtBoard.FindAll(fig => !fig.Equals(king));
            Console.WriteLine(figuresWithoutKing.ToArray().ToString());
            foreach (Figure figure in figuresWithoutKing)
            {
                if (!figure.Player.Equals(king.Player) &&
                    GetAllowedMoves(figuresWithoutKing, figure).Contains(king.Position)) return true;
            }
            return false;
        }

        public Figure? FigureAt(Vector2 position)
        {
            return Board.FiguresAtBoard.Find(fig => fig.Position.Equals(position));
        }
    }
}