using ChessGameApi.Entities;
using ChessGameApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChessGameApi.Repositories
{
    public class MongoDBFiguresRepository : IFigureRepository
    {
        private const string databaseName = "chess";
        private const string collectionName = "figures";
        private readonly IMongoCollection<FigureEntity> _figures;
        private readonly FilterDefinitionBuilder<FigureEntity> _filterBuilder = Builders<FigureEntity>.Filter;

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
            var filter = _filterBuilder.Eq(fig => fig.Id, id);
            _figures.DeleteOne(filter);
        }

        public FigureEntity GetFigure(Guid id)
        {
            var filter = _filterBuilder.Eq(fig => fig.Id, id);
            return _figures.Find(filter).SingleOrDefault();
        }

        public IEnumerable<FigureEntity> GetFigures()
        {
            return _figures.Find(new BsonDocument()).ToList();
        }

        public void UpdateFigure(FigureEntity figure)
        {
            var filter = _filterBuilder.Eq(existFig=> existFig.Id, figure.Id);
            _figures.ReplaceOne(filter, figure);
        }
    }
}
