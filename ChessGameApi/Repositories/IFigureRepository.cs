using ChessGameApi.Entities;

namespace ChessGameApi.Repositories
{
    public interface IFigureRepository
    {
        FigureEntity GetFigure(Guid id);
        IEnumerable<FigureEntity> GetFigures();
        void CreateFigure(FigureEntity figure);
        void UpdateFigure(FigureEntity figure);
        void DeleteFigure(Guid id);
    }
}