using ChessGameApi.Entities;

namespace ChessGameApi.Repositories
{
    public interface IFigureRepository
    {
        Task<FigureEntity?> GetFigureAsync(Guid id);
        Task<IEnumerable<FigureEntity>> GetFiguresAsync();
        Task CreateFigureAsync(FigureEntity figure);
        Task UpdateFigureAsync(FigureEntity figure);
        Task DeleteFigureAsync(Guid id);
        Task DeleteFigureAtPositionAsync(int x, int y);
        Task<bool> HasFigureAtAsync(int x, int y);
    }
}