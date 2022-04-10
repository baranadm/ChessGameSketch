namespace ChessGameApi.Entities
{
    public record FieldEntity
    {
        public Guid Id { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
    }
}
