using System.ComponentModel.DataAnnotations;

namespace ChessGameApi.Dtos
{
    public record MoveFigureDto
    {
        [Required]
        [Range(0, 7)]
        public int X { get; init; }
        [Required]
        [Range(0, 7)]
        public int Y { get; init; }
    }
}
