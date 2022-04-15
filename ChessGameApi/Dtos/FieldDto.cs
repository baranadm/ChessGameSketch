using System.ComponentModel.DataAnnotations;

namespace ChessGameApi.Dtos
{
    public record FieldDto
    {
        [Required]
        [Range(0, 7)]
        public int X { get; init; }
        [Required]
        [Range(0, 7)]
        public int Y { get; init; }
    }
}
