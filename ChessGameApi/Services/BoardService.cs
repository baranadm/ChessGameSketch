using ChessGameApi.Dtos;
using ChessGameApi.Entities;
using ChessGameApi.Exceptions;
using ChessGameApi.Extensions;
using ChessGameApi.Repositories;
using ChessGameSketch;
using ChessGameSketch.Validator;
using System.Numerics;

namespace ChessGameApi.Services
{
    public class BoardService : IBoardService
    {
        private readonly IFigureRepository figureRepository;
        private readonly IEnPassantRepository enPassantRepository;
        private readonly IChessValidator chessValidator;

        public BoardService(IFigureRepository figureRepository, IEnPassantRepository enPassantRepository, IChessValidator chessValidator)
        {
            this.figureRepository = figureRepository;
            this.enPassantRepository = enPassantRepository;
            this.chessValidator = chessValidator;
        }

        public async Task<List<FigureDto>> GetFiguresAsync()
        {
            var figures = (await figureRepository.GetFiguresAsync())
                .Select(fig => fig.AsDto());
            return new List<FigureDto>(figures);
        }

        public async Task<FigureDto?> GetFigureAsync(Guid id)
        {
            var figure = await figureRepository.GetFigureAsync(id);
            return figure?.AsDto();
        }

        public List<NewFigureDto> GetNewFigures()
        {
            List<NewFigureDto> figures = new List<NewFigureDto>()
            {
                new NewFigureDto() { Player = "White", FigureType = "Pawn" },
                new NewFigureDto() { Player = "White", FigureType = "Knight" },
                new NewFigureDto() { Player = "White", FigureType = "Bishop" },
                new NewFigureDto() { Player = "White", FigureType = "Rook" },
                new NewFigureDto() { Player = "White", FigureType = "Queen" },
                new NewFigureDto() { Player = "White", FigureType = "King" },
                new NewFigureDto() { Player = "Black", FigureType = "Pawn" },
                new NewFigureDto() { Player = "Black", FigureType = "Knight" },
                new NewFigureDto() { Player = "Black", FigureType = "Bishop" },
                new NewFigureDto() { Player = "Black", FigureType = "Rook" },
                new NewFigureDto() { Player = "Black", FigureType = "Queen" },
                new NewFigureDto() { Player = "Black", FigureType = "King" }
            };
            return figures;
        }
        public async Task PutNewFigureAsync(CreateFigureDto newFigureDto)
        {
            if (await figureRepository.HasFigureAtAsync(newFigureDto.X, newFigureDto.Y))
                throw new FieldOccupiedException($"Field [{newFigureDto.X}, {newFigureDto.Y}] is occupied by another figure");

            FigureEntity figureEntity = new()
            {
                Id = Guid.NewGuid(),
                X = newFigureDto.X,
                Y = newFigureDto.Y,
                Player = newFigureDto.Player,
                FigureType = newFigureDto.FigureType
            };

            await figureRepository.CreateFigureAsync(figureEntity);
        }

        public async Task MoveFigureAsync(Guid id, FieldDto newPosition)
        {
            FigureEntity? existingFigure = await figureRepository.GetFigureAsync(id);
            if (existingFigure is null)
            {
                throw new FigureNotFoundException($"Figure with id={id} has not been found.");
            }

            List<Vector2> allowedMoves = await FindAllowedMoves(existingFigure);
            Vector2 newPositionAsVector = new Vector2(newPosition.X, newPosition.Y);
            if (!allowedMoves.Contains(newPositionAsVector))
            {
                throw new MoveNotAllowedException($"Move from [{existingFigure.X}, {existingFigure.Y}] to [{newPosition.X}, {newPosition.Y}] is not allowed.");
            }

            await handleEnPassant(newPosition, existingFigure);
            await takeFigureIfExists(newPosition);

            FigureEntity afterMove = existingFigure with
            {
                X = newPosition.X,
                Y = newPosition.Y,
            };
            await figureRepository.UpdateFigureAsync(afterMove);
        }

        private async Task takeFigureIfExists(FieldDto newPosition)
        {
            try
            {
                await figureRepository.DeleteFigureAtPositionAsync(newPosition.X, newPosition.Y);
            }
            catch (FigureNotFoundException)
            { }
        }

        private async Task handleEnPassant(FieldDto newPosition, FigureEntity existingFigure)
        {
            await enPassantRepository.ClearEnPassantFieldAsync();
            if (existingFigure.FigureType == "Pawn")
            {
                if (Math.Abs(existingFigure.Y - newPosition.Y) == 2)
                {
                    FieldEntity enPassantField = new()
                    {
                        Id = Guid.NewGuid(),
                        X = existingFigure.X,
                        Y = (existingFigure.Y + newPosition.Y) / 2
                    };
                    await enPassantRepository.NewEnPassantFieldAsync(enPassantField);
                }
            }
        }

        public async Task DeleteFigureAsync(Guid id)
        {
            try
            {
                await figureRepository.DeleteFigureAsync(id);
            }
            catch (FigureNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<FieldDto>> GetAllowedMoves(Guid id)
        {
            FigureEntity? figure = await figureRepository.GetFigureAsync(id);
            if(figure is null)
            {
                throw new FigureNotFoundException($"Figure with id={id} has not been found.");
            }

            List<Vector2> allowedMoves = await FindAllowedMoves(figure);
            return allowedMoves.Select(vec => vec.AsFieldDto()).ToList();
        }

        private async Task<List<Vector2>> FindAllowedMoves(FigureEntity figure)
        {
            List<Figure> figuresOnBoard = (await figureRepository.GetFiguresAsync()).Select(fig => fig.AsFigure()).ToList();
            Board board = new Board(figuresOnBoard);

            FieldEntity? enPassant = await enPassantRepository.GetEnPassantFieldAsync();
            if (enPassant != null) board.EnPassantField = enPassant.AsVector2();

            Figure figureInspected = figure.AsFigure();
            List<Vector2> allowedMoves = chessValidator.GetAllowedMoves(board, figureInspected);
            return allowedMoves;
        }
    }
}
