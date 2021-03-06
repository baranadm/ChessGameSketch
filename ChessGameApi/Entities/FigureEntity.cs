namespace ChessGameApi.Entities
{
    public record FigureEntity
    {
        public Guid Id { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public string Player { get; init; }
        public string FigureType { get; init; }
    }
}
