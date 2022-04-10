using ChessGameApi.Entities;
using ChessGameApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChessGameApi.Repositories
{
    public class MongoDBFiguresRepository : IFigureRepository
    {
        private const string databaseName = "chess";
        private const string collectionName = "figures";
        private readonly IMongoCollection<FigureEntity> _figures;

        public MongoDBFiguresRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            _figures = database.GetCollection<FigureEntity>(collectionName);
        }

        public void CreateFigure(FigureEntity figure)
        {
            _figures.InsertOne(figure);
        }

        public void DeleteFigure(Guid id)
        {
            throw new NotImplementedException();
        }

        public FigureEntity GetFigure(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FigureEntity> GetFigures()
        {
            throw new NotImplementedException();
        }

        public void UpdateFigure(FigureEntity figure)
        {
            throw new NotImplementedException();
        }
    }
}
