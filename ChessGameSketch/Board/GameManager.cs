using System.Numerics;

namespace ChessGameSketch
{
    public class GameManager
    {

        public List<Vector2> GetAllowedMoves(Board board, Figure figureInspected)
        {
            List<Vector2> possibleMoves = new List<Vector2>();
            possibleMoves.AddRange(GetAttackedFields(figureInspected, board));
            
            List<Vector2> movesCausingCheck = new List<Vector2>();

            // if current player has king on the board, simulate every move and dismiss every move, that causes check on king
            // check, if there are player's kings
            List<Figure> friendlyKings = board.FindAll(FigureType.King, figureInspected.Player);
            if (friendlyKings.Any())
            {
                Player opponent = figureInspected.Player.Equals(Player.White) ? Player.Black : Player.White;

                foreach(Vector2 possibleMove in possibleMoves)
                {
                    Board simulatedBoard = board.GetCopy();
                    List<Figure> simulatedOpponentsFigures = simulatedBoard.FindAll(opponent);
                    Figure? simulatedFigureInspected = simulatedBoard.FigureOn(figureInspected.Position);

                    simulatedBoard.MoveFigure(simulatedFigureInspected, possibleMove);
                    List<Figure> simulatedFriendlyKings = simulatedBoard.FindAll(FigureType.King,figureInspected.Player);
                    List<Vector2> simulatedFriendlyKingsPositions = simulatedFriendlyKings.Select(king => king.Position).ToList();

                    bool isAnyKingChecked = simulatedOpponentsFigures.Exists(fig => GetAttackedFields(fig, simulatedBoard).Exists(field => simulatedFriendlyKingsPositions.Contains(field)));
                    if(isAnyKingChecked) movesCausingCheck.Add(possibleMove);
                }
            }

            possibleMoves.RemoveAll(move => movesCausingCheck.Contains(move));
            return possibleMoves;
        }



        private List<Vector2> GetAttackedFields(Figure figureInspected, Board board)
        {
            FigureMoves figureMoves = figureInspected.GetFigureMoves();
            List<Vector2> figureMovesDirections = figureMoves.Directions;

            Pawn? pawn = figureInspected.GetFigureType().Equals(FigureType.Pawn) ? (Pawn) figureInspected : null;
            if (pawn != null)
            {
                foreach(Vector2 attackDirection in pawn.GetAttackDirections().Directions)
                {
                    Vector2 possibleAttackField = Vector2.Add(pawn.Position, attackDirection);
                    Figure? possibleFigureAttacked = board.FigureOn(possibleAttackField);
                    if (!possibleFigureAttacked.SamePlayerAs(pawn))
                    {
                        figureMovesDirections.Add(attackDirection);
                    }
                }                
            }
            List<Vector2> attackedFields = new List<Vector2>();
            for (int i = 0; i < figureMovesDirections.Count; i++)
            {
                Vector2 actualDirection = figureMovesDirections[i];
                Vector2 actualPosition = figureInspected.Position;

                while (true)
                {
                    Vector2 nextPosition = Vector2.Add(actualPosition, actualDirection);

                    bool outOfBounds = nextPosition.X < 0 || nextPosition.X > 7 || nextPosition.Y < 0 || nextPosition.Y > 7;
                    if (outOfBounds) break;

                    //break if next tile is occupied by another figure with same color (player)
                    Figure? figureOnNextPosition = board.FigureOn(nextPosition);
                    if (figureOnNextPosition != null)
                    {
                        bool samePlayer = figureOnNextPosition.SamePlayerAs(figureInspected);
                        if (samePlayer) break;
                        else
                        {
                            attackedFields.Add(nextPosition);
                            break;
                        }
                    } else
                    {
                        attackedFields.Add(nextPosition);
                        actualPosition.X = nextPosition.X;
                        actualPosition.Y = nextPosition.Y;
                        //board.MoveFigure(figureInspected, nextPosition);
                    }

                    // break if move is not repetable
                    if (!figureMoves.Repeatable) break;
                }
            }

            return attackedFields;
        }

    }
}