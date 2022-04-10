using System.ComponentModel.DataAnnotations;

namespace ChessGameApi.Dtos
{
    public record UpdateFigureDto
    {
        [Required]
        [Range(0, 7)]
        public int X { get; init; }
        [Required]
        [Range(0, 7)]
        public int Y { get; init; }
        [Required]
        [RegularExpression("White|Black", ErrorMessage = "Player should be either 'White' or 'Black'.")]
        public string Player { get; init; }
        [Required]
        [RegularExpression("Pawn|Knight|Bishop|Rook|Queen|King", ErrorMessage = "Allowed figure types: 'Pawn', 'Knight', 'Bishop', 'Rook', 'Queen', 'King'")]
        public string FigureType { get; init; }
    }
}
