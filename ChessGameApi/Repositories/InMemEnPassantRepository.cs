using ChessGameApi.Entities;
using System.Numerics;

namespace ChessGameApi.Repositories
{
    public class InMemEnPassantRepository : IEnPassantRepository
    {
        FieldEntity? enPassantField;
        public Task ClearEnPassantFieldAsync()
        {
            this.enPassantField = null;
            return Task.CompletedTask;
        }

        public Task<FieldEntity?> GetEnPassantFieldAsync()
        {
            return Task.FromResult(enPassantField);
        }

        public Task NewEnPassantFieldAsync(FieldEntity enPassantField)
        {
            this.enPassantField = enPassantField;
            return Task.CompletedTask;
        }
    }
}
