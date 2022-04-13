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

        public async Task CreateFigureAsync(FigureEntity figure)
        {
            await _figures.InsertOneAsync(figure);
        }

        public async Task DeleteFigureAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(fig => fig.Id, id);
            await _figures.DeleteOneAsync(filter);
        }

        public async Task<FigureEntity> GetFigureAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(fig => fig.Id, id);
            return await _figures.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<FigureEntity>> GetFiguresAsync()
        {
            return await _figures.Find(new BsonDocument()).ToListAsync();
        }

        public async Task DeleteFigureAtPositionAsync(int x, int y)
        {
            var filter = _filterBuilder.Where(fig => fig.X == x && fig.Y == y);
            await _figures.DeleteOneAsync(filter);
        }

        public async Task UpdateFigureAsync(FigureEntity figure)
        {
            var filter = _filterBuilder.Eq(existFig=> existFig.Id, figure.Id);
            await _figures.ReplaceOneAsync(filter, figure);
        }
    }
}
