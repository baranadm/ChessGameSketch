using ChessGameApi.Entities;
using System.Numerics;

namespace ChessGameApi.Repositories
{
    public interface IEnPassantRepository
    {
        public Task<FieldEntity?> GetEnPassantFieldAsync();
        public Task NewEnPassantFieldAsync(FieldEntity enPassantField);
        public Task ClearEnPassantFieldAsync();
    }
}
