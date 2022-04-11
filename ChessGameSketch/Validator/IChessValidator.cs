using System.Numerics;

namespace ChessGameSketch.Validator
{
    public interface IChessValidator
    {
        public List<Vector2> GetAllowedMoves(Board board, Figure figure);
    }
}
