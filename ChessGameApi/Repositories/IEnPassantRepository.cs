using ChessGameApi.Entities;
using System.Numerics;

namespace ChessGameApi.Repositories
{
    public interface IEnPassantRepository
    {
        public Task<Field> GetEnPassantFieldAsync();
        public Task NewEnPassantFieldAsync(Field enPassantField);
        public Task ClearEnPassantFieldAsync();
    }
}
