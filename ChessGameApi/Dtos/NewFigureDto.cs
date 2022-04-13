namespace ChessGameApi.Dtos
{
    public record NewFigureDto
    {
        public string Player { get; init; }
        public string FigureType { get; init; }
    }
}
