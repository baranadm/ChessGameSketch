using ChessGameApi.Entities;

namespace ChessGameApi.Repositories
{
    public class InMemFigureRepository : IFigureRepository
    {
        private readonly List<FigureEntity> figures = new()
        {
            new FigureEntity { Id = Guid.NewGuid(), X = 0, Y = 1, FigureType = "Pawn", Player = "White" },
            new FigureEntity { Id = Guid.NewGuid(), X = 1, Y = 1, FigureType = "Pawn", Player = "White" },
            new FigureEntity { Id = Guid.NewGuid(), X = 0, Y = 0, FigureType = "Rook", Player = "White" }
        };

        public async Task<IEnumerable<FigureEntity>> GetFiguresAsync()
        {
            return await Task.FromResult(figures);
        }

        public async Task<FigureEntity> GetFigureAsync(Guid id)
        {
            var result = figures.Where(f => f.Id == id).SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task CreateFigureAsync(FigureEntity figure)
        {
            figures.Add(figure);
            await Task.CompletedTask;
        }

        public async Task UpdateFigureAsync(FigureEntity figure)
        {
            var index = figures.FindIndex(fig => fig.Id == figure.Id);
            figures[index] = figure;
            await Task.CompletedTask;
        }

        public async Task DeleteFigureAsync(Guid id)
        {
            var index = figures.FindIndex(fig => fig.Id == id);
            figures.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}