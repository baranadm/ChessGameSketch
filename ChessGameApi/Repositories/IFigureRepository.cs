using ChessGameApi.Entities;

namespace ChessGameApi.Repositories
{
    public interface IFigureRepository
    {
        FigureEntity GetFigure(long id);
        IEnumerable<FigureEntity> GetFigures();
    }
}