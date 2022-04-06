using System.Numerics;

namespace ChessGameSketch
{
    public class GameManager
    {

        public List<Vector2> GetAllowedMoves(Board board, Figure figureInspected)
        {
            List<Vector2> possibleMoves = new List<Vector2>();
            possibleMoves.AddRange(GetAccessiblePositions(figureInspected, board));

            List<Vector2> movesCausingCheck = new List<Vector2>();

            List<Figure> friendlyKings = board.FindAll(FigureType.King, figureInspected.Player);
            if (friendlyKings.Any())
            {
                Player opponent = figureInspected.Player.Equals(Player.White) ? Player.Black : Player.White;

                foreach (Vector2 possibleMove in possibleMoves)
                {
                    Board simulatedBoard = board.GetCopy();
                    List<Figure> simulatedOpponentsFigures = simulatedBoard.FindAll(opponent);
                    Figure? simulatedFigureInspected = simulatedBoard.FigureOn(figureInspected.Position);

                    simulatedBoard.MoveFigure(simulatedFigureInspected, possibleMove);
                    List<Figure> simulatedFriendlyKings = simulatedBoard.FindAll(FigureType.King, figureInspected.Player);
                    List<Vector2> simulatedFriendlyKingsPositions = simulatedFriendlyKings.Select(king => king.Position).ToList();

                    bool isAnyKingChecked = simulatedOpponentsFigures.Exists(fig => GetAccessiblePositions(fig, simulatedBoard).Exists(field => simulatedFriendlyKingsPositions.Contains(field)));
                    if (isAnyKingChecked) movesCausingCheck.Add(possibleMove);
                }
            }

            possibleMoves.RemoveAll(move => movesCausingCheck.Contains(move));
            return possibleMoves;
        }



        private List<Vector2> GetAccessiblePositions(Figure figureInspected, Board board)
        {
            List<FigureMove> figureMoves = figureInspected.GetFigureMoves();

            Pawn? pawn = figureInspected.GetFigureType().Equals(FigureType.Pawn) ? (Pawn)figureInspected : null;
            if (pawn != null)
            {
                foreach (FigureMove attackMove in pawn.GetAttackMoves())
                {
                    Vector2 positionUnderAttack = Vector2.Add(pawn.Position, attackMove.Direction);
                    Figure? figureUnderAttack = board.FigureOn(positionUnderAttack);
                    if (figureUnderAttack != null && !figureUnderAttack.SamePlayerAs(pawn))
                    {
                        figureMoves.Add(attackMove);
                    }
                }
            }

            List<Vector2> accessiblePositions = new List<Vector2>();
            for (int i = 0; i < figureMoves.Count; i++)
            {
                FigureMove actualMove = figureMoves[i];
                Vector2 actualPosition = figureInspected.Position;

                while (true)
                {
                    Vector2 nextPosition = Vector2.Add(actualPosition, actualMove.Direction);

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
                            accessiblePositions.Add(nextPosition);
                            break;
                        }
                    }
                    else
                    {
                        accessiblePositions.Add(nextPosition);
                        actualPosition.X = nextPosition.X;
                        actualPosition.Y = nextPosition.Y;
                    }

                    // break if move is not repetable
                    if (!actualMove.Repeatable) break;
                }
            }

            return accessiblePositions;
        }

    }
}