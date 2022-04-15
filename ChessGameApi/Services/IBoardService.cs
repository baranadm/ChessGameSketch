using ChessGameApi.Dtos;

namespace ChessGameApi.Services
{
    public interface IBoardService
    {
        public Task<List<FigureDto>> GetFiguresAsync();
        public Task<FigureDto?> GetFigureAsync(Guid id);
        public List<NewFigureDto> GetNewFigures();
        public Task PutNewFigureAsync(CreateFigureDto newFigureDto);
        public Task MoveFigureAsync(Guid id, FieldDto newPosition);
        public Task DeleteFigureAsync(Guid id);
        public Task<List<FieldDto>> GetAllowedMoves(Guid id);
    }
}
