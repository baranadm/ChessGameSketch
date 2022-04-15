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

        public IEnumerable<FigureEntity> GetFigures()
        {
            return figures;
        }

        public FigureEntity GetFigure(Guid id)
        {
            return figures.Where(f => f.Id == id).SingleOrDefault();
        }

        public void CreateFigure(FigureEntity figure)
        {
            figures.Add(figure);
        }

        public void UpdateFigure(FigureEntity figure)
        {
            var index = figures.FindIndex(fig => fig.Id == figure.Id);
            figures[index] = figure;
        }

        public void DeleteFigure(Guid id)
        {
            var index = figures.FindIndex(fig => fig.Id == id);
            figures.RemoveAt(index);
        }
    }
}