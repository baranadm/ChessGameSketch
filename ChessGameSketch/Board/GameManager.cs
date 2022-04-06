using System.Numerics;

namespace ChessGameSketch
{
    public class GameManager
    {

        public List<Vector2> GetAllowedMoves(Board board, Figure figureInspected)
        {
            List<Vector2> possibleMoves = GetAccessiblePositions(figureInspected, board);

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

        private List<Vector2> GetAccessiblePositions(Figure inspectedFigure, Board board)
        {
            List<FigureMove> inspectedFigureMoves = AvailableMovesFor(inspectedFigure, board);

            List<Vector2> accessiblePositions = new List<Vector2>();
            foreach(FigureMove actualMove in inspectedFigureMoves)
            {
                Vector2 actualPosition = inspectedFigure.Position;

                while (true)
                {
                    Vector2 nextPosition = Vector2.Add(actualPosition, actualMove.Direction);

                    if (IsOutOfBounds(nextPosition)) break;

                    //break if next tile is occupied by another figure with same color (player)
                    Figure? figureOnNextPosition = board.FigureOn(nextPosition);
                    if (figureOnNextPosition != null)
                    {
                        if (figureOnNextPosition.SamePlayerAs(inspectedFigure)) break;
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

        public static bool IsOutOfBounds(Vector2 nextPosition)
        {
            return nextPosition.X < 0 || nextPosition.X > 7 || nextPosition.Y < 0 || nextPosition.Y > 7;
        }

        private List<FigureMove> AvailableMovesFor(Figure inspectedFigure, Board board)
        {
            List<FigureMove> inspectedFigureMoves = inspectedFigure.GetFigureMoves();
            if (inspectedFigure.IsType(FigureType.Pawn))
            {
                Pawn pawn = (Pawn) inspectedFigure;
                inspectedFigureMoves.AddRange(GetPawnsPossibleAttackMoves(pawn, board));
            }

            return inspectedFigureMoves;
        }

        private List<FigureMove> GetPawnsPossibleAttackMoves(Pawn pawn, Board board)
        {
            List<FigureMove> pawnsAttackMoves = new List<FigureMove>();
            foreach (FigureMove attackMove in pawn.GetAttackMoves())
            {
                Vector2 positionUnderAttack = Vector2.Add(pawn.Position, attackMove.Direction);
                Figure? figureUnderAttack = board.FigureOn(positionUnderAttack);
                
                if (figureUnderAttack != null && !figureUnderAttack.SamePlayerAs(pawn)) 
                    pawnsAttackMoves.Add(attackMove);

                if (positionUnderAttack == board.EnPassantField)
                    pawnsAttackMoves.Add(attackMove);
            }
            return pawnsAttackMoves;
        }

    }
}