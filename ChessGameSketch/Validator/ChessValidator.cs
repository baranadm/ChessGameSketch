using System.Numerics;

namespace ChessGameSketch.Validator
{
    public class ChessValidator : IChessValidator
    {

        public List<Vector2> GetAllowedMoves(Board board, Figure figureInspected)
        {
            List<Vector2> possibleMoves = GetAccessiblePositions(figureInspected, board);

            List<Vector2> movesCausingCheck = new List<Vector2>();

            List<Figure> friendlyKings = board.FindPlayersFiguresWithType(FigureType.King, figureInspected.Player);
            if (friendlyKings.Any())
            {
                Player opponent = figureInspected.Player.Equals(Player.White) ? Player.Black : Player.White;

                foreach (Vector2 possibleMove in possibleMoves)
                {
                    Board simulatedBoard = board.GetCopy();
                    List<Figure> simulatedOpponentsFigures = simulatedBoard.FindPlayersFigures(opponent);
                    Figure? simulatedFigureInspected = simulatedBoard.FigureOn(figureInspected.Position);

                    simulatedBoard.MoveFigure(simulatedFigureInspected, possibleMove);
                    List<Figure> simulatedFriendlyKings = simulatedBoard.FindPlayersFiguresWithType(FigureType.King, figureInspected.Player);
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
            List<MoveDirection> inspectedFigureMoves = ContextDependentMovesFor(inspectedFigure, board);

            List<Vector2> accessiblePositions = new List<Vector2>();
            foreach (MoveDirection actualDirection in inspectedFigureMoves)
            {
                accessiblePositions.AddRange(
                    FindAccassiblePositionsInDirection(actualDirection, inspectedFigure, board));
            }

            return accessiblePositions;
        }

        private List<Vector2> FindAccassiblePositionsInDirection(MoveDirection actualDirection, Figure inspectedFigure, Board board)
        {
            List<Vector2> result = new List<Vector2>();

            Vector2 actualPosition = inspectedFigure.Position;
            while (true)
            {
                Vector2 nextPosition = Vector2.Add(actualPosition, actualDirection.Step);

                if (IsOutOfBounds(nextPosition)) break;
                if (WillOverlapFriend(inspectedFigure, nextPosition, board)) break;
                else if (WillOverlapOpponent(inspectedFigure, nextPosition, board))
                {
                    result.Add(nextPosition);
                    break;
                }
                else
                {
                    result.Add(nextPosition);
                    actualPosition.X = nextPosition.X;
                    actualPosition.Y = nextPosition.Y;
                }
                if (!actualDirection.Repeatable) break;
            }
            return result;
        }

        private bool IsOutOfBounds(Vector2 nextPosition)
        {
            return nextPosition.X < 0 || nextPosition.X > 7 || nextPosition.Y < 0 || nextPosition.Y > 7;
        }
        
        private bool WillOverlapFriend(Figure inspectedFigure, Vector2 desiredPosition, Board board)
        {
            Figure? figureOnDesiredPosition = board.FigureOn(desiredPosition);
            if (figureOnDesiredPosition != null && figureOnDesiredPosition.SamePlayerAs(inspectedFigure))
            {
                return true;
            }
            return false;
        }
        
        private bool WillOverlapOpponent(Figure inspectedFigure, Vector2 desiredPosition, Board board)
        {
            Figure? figureOnDesiredPosition = board.FigureOn(desiredPosition);
            if (figureOnDesiredPosition != null && !figureOnDesiredPosition.SamePlayerAs(inspectedFigure))
            {
                return true;
            }
            return false;
        }
        
        private List<MoveDirection> ContextDependentMovesFor(Figure inspectedFigure, Board board)
        {
            List<MoveDirection> inspectedFigureMoves = inspectedFigure.GetFigureMoves();
            if (inspectedFigure.IsType(FigureType.Pawn))
            {
                Pawn pawn = (Pawn)inspectedFigure;
                inspectedFigureMoves.AddRange(GetPawnsPossibleAttackMoves(pawn, board));
            }

            return inspectedFigureMoves;
        }

        private List<MoveDirection> GetPawnsPossibleAttackMoves(Pawn pawn, Board board)
        {
            List<MoveDirection> pawnsAttackMoves = new List<MoveDirection>();
            foreach (MoveDirection attackMove in pawn.GetAttackMoves())
            {
                Vector2 positionUnderAttack = Vector2.Add(pawn.Position, attackMove.Step);
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