using ChessGameApi.Dtos;
using ChessGameApi.Entities;

namespace ChessGameApi.Extensions
{
    public static class Extensions
    {
        public static FigureDto AsDto(this FigureEntity figureEntity)
        {
            return new FigureDto
            {
                Id = figureEntity.Id,
                X = figureEntity.X,
                Y = figureEntity.Y,
                Player = figureEntity.Player,
                FigureType = figureEntity.FigureType
            };
        }
    }
}
