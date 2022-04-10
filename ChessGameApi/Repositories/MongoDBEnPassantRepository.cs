using ChessGameApi.Entities;
using ChessGameApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Numerics;

namespace ChessGameApi.Repositories
{
    public class MongoDBEnPassantRepository : IEnPassantRepository
    {
        private const string databaseName = "chess";
        private const string collectionName = "enPassantFields";
        private readonly IMongoCollection<FieldEntity> _enPassantFields;
        private readonly FilterDefinitionBuilder<FieldEntity> _filterBuilder = Builders<FieldEntity>.Filter;

        public MongoDBEnPassantRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            _enPassantFields = database.GetCollection<FieldEntity>(collectionName);
        }

        public async Task<FieldEntity> GetEnPassantFieldAsync()
        {
            List<FieldEntity> fields = await _enPassantFields.Find(new BsonDocument()).ToListAsync();
            if(fields.Count > 1)
            {
                throw new Exception("More than one en passant fields found.");
            }
            return fields[0];
        }

        public async Task NewEnPassantFieldAsync(FieldEntity enPassantField)
        {
            await _enPassantFields.DeleteManyAsync(new BsonDocument());
            await _enPassantFields.InsertOneAsync(enPassantField);
        }

        public async Task ClearEnPassantFieldAsync()
        {
            await _enPassantFields.DeleteManyAsync(new BsonDocument());
        }

    }
}
