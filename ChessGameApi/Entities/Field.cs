namespace ChessGameApi.Entities
{
    public record Field
    {
        public Guid Id { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
    }
}
