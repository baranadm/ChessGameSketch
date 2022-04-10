using ChessGameApi.Entities;

namespace ChessGameApi.Repositories
{
    public class InMemFigureRepository : IFigureRepository
    {
        private readonly List<FigureEntity> figures = new()
        {
            new FigureEntity { Id = 1, X = 0, Y = 1, FigureType = "Pawn", Player = "White" },
            new FigureEntity { Id = 2, X = 1, Y = 1, FigureType = "Pawn", Player = "White" },
            new FigureEntity { Id = 3, X = 0, Y = 0, FigureType = "Rook", Player = "White" }
        };

        public IEnumerable<FigureEntity> GetFigures()
        {
            return figures;
        }

        public FigureEntity GetFigure(long id)
        {
            return figures.Where(f => f.Id == id).SingleOrDefault();
        }
    }
}
