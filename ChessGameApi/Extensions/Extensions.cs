using ChessGameApi.Dtos;
using ChessGameApi.Entities;
using ChessGameSketch;
using System.Numerics;

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

        public static FigureEntity AsEntity(this FigureDto figureDto)
        {
            return new FigureEntity
            {
                Id = figureDto.Id,
                X = figureDto.X,
                Y = figureDto.Y,
                Player = figureDto.Player,
                FigureType = figureDto.FigureType
            };
        }

        public static Figure AsFigure(this FigureEntity figureEntity)
        {
            Player player;
            if (figureEntity.Player != null && figureEntity.Player.Equals("White")) player = Player.White;
            else if (figureEntity.Player != null && figureEntity.Player.Equals("Black")) player = Player.Black;
            else throw new Exception("Unknown player.");

            if (figureEntity.FigureType != null)
            {
                Vector2 position = new Vector2(figureEntity.X, figureEntity.Y);

                if (figureEntity.FigureType.Equals("Pawn")) return new Pawn(position, player);
                if (figureEntity.FigureType.Equals("Knight")) return new Knight(position, player);
                if (figureEntity.FigureType.Equals("Bishop")) return new Bishop(position, player);
                if (figureEntity.FigureType.Equals("Rook")) return new Rook(position, player);
                if (figureEntity.FigureType.Equals("Queen")) return new Queen(position, player);
                if (figureEntity.FigureType.Equals("King")) return new King(position, player);
                else throw new Exception("Unknown figure type.");
            }
            else
            {
                throw new Exception("Figure type not specified.");
            }
        }

        public static Figure AsFigure(this FigureDto figureDto)
        {
            return AsFigure(figureDto.AsEntity());
        }

        public static Vector2 AsVector2(this FieldEntity field)
        {
            return new Vector2(field.X, field.Y);
        }
        public static FieldDto AsFieldDto(this Vector2 vector)
        {
            return new FieldDto()
            {
                X = (int) vector.X,
                Y = (int) vector.Y
            };
        }

    }
}
