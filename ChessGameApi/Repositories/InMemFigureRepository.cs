using ChessGameApi.Entities;
using ChessGameApi.Exceptions;

namespace ChessGameApi.Repositories
{
    public class InMemFigureRepository : IFigureRepository
    {
        private readonly List<FigureEntity> figures = new();

        public Task<FigureEntity?> GetFigureAsync(Guid id)
        {
            return Task.FromResult(figures.SingleOrDefault(f => f.Id == id));
        }

        public Task<IEnumerable<FigureEntity>> GetFiguresAsync()
        {
            return Task.FromResult(figures.AsEnumerable());
        }

        public Task CreateFigureAsync(FigureEntity figure)
        {
            figures.Add(figure);
            return Task.CompletedTask;
        }

        public Task UpdateFigureAsync(FigureEntity figure)
        {
            var index = figures.FindIndex(fig => fig.Id == figure.Id);
            if (index == -1)
            {
                throw new FigureNotFoundException($"Figure with id={figure.Id} has not been found.");
            }
            figures[index] = figure;
            return Task.CompletedTask;
        }

        public Task DeleteFigureAsync(Guid id)
        {
            var index = figures.FindIndex(fig => fig.Id == id);
            try
            {
                figures.RemoveAt(index);
                return Task.CompletedTask;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FigureNotFoundException($"Figure with id={id} has not been found.");
            }
        }

        public Task DeleteFigureAtPositionAsync(int x, int y)
        {
            var index = figures.FindIndex((fig) => fig.X == x && fig.Y == y);

            try
            {
                figures.RemoveAt(index);
                return Task.CompletedTask;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FigureNotFoundException($"Figure with x={x}, y={y} has not been found.");
            }
        }

        public Task<bool> HasFigureAtAsync(int x, int y)
        {
            return Task.FromResult(figures.Exists((fig) => fig.X == x && fig.Y == y));
        }
    }
}