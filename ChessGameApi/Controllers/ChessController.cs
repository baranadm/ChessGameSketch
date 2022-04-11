using ChessGameApi.Dtos;
using ChessGameApi.Entities;
using ChessGameApi.Extensions;
using ChessGameApi.Repositories;
using ChessGameSketch;
using ChessGameSketch.Validator;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace ChessGameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChessController : ControllerBase
    {
        private readonly IFigureRepository figureRepository;
        private readonly IEnPassantRepository enPassantRepository;
        private readonly IChessValidator chessValidator;

        public ChessController(IFigureRepository figureRepository, IEnPassantRepository enPassantRepository, IChessValidator chessValidator)
        {
            this.figureRepository = figureRepository;
            this.enPassantRepository = enPassantRepository;
            this.chessValidator = chessValidator;
        }

        //GET /chess
        [HttpGet]
        public async Task <IEnumerable<FigureDto>> GetFiguresAsync()
        {
            var figures = (await figureRepository.GetFiguresAsync())
                .Select(fig => fig.AsDto());
            return figures;
        }

        //GET /chess/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FigureDto>> GetFigureAsync(Guid id)
        {
            var figure = await figureRepository.GetFigureAsync(id);
            if (figure is null)
            {
                return NotFound();
            }
            return figure.AsDto();
        }

        // POST /chess
        [HttpPost]
        public async Task<ActionResult<FigureDto>> CreateFigureAsync(CreateFigureDto newFigureDto)
        {
            FigureEntity figureEntity = new()
            {
                Id = Guid.NewGuid(),
                X = newFigureDto.X,
                Y = newFigureDto.Y,
                Player = newFigureDto.Player,
                FigureType = newFigureDto.FigureType
            };

            FieldEntity enPassantField = new()
            {
                Id = Guid.NewGuid(),
                X = newFigureDto.X,
                Y = newFigureDto.Y
            };
            await enPassantRepository.NewEnPassantFieldAsync(enPassantField);

            await figureRepository.CreateFigureAsync(figureEntity);
            return CreatedAtAction(nameof(GetFigureAsync), new { id = figureEntity.Id }, figureEntity.AsDto());
        }

        // PUT /chess/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> MoveFigureAsync(Guid id, UpdateFigureDto figureDto)
        {
            var existingFigure = await figureRepository.GetFigureAsync(id);
            if(existingFigure is null)
            {
                return NotFound();
            }


            FigureEntity updatedFigure = existingFigure with
            {
                X = figureDto.X,
                Y = figureDto.Y,
                Player = figureDto.Player,
                FigureType = figureDto.FigureType
            };

            await figureRepository.UpdateFigureAsync(updatedFigure);
            return NoContent();
        }

        // DELETE /Chess/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFigureAsync(Guid id)
        {
            var existingFigure = await figureRepository.GetFigureAsync(id);
            if (existingFigure is null)
            {
                return NotFound();
            }

            await figureRepository.DeleteFigureAsync(id);
            return NoContent();
        }
        
        [HttpGet("getAllowedMoves/{id}")]
        public async Task<ActionResult<List<FieldDto>>> GetAllowedPositions(Guid id)
        {

            List<Figure> figuresOnBoard = (await figureRepository.GetFiguresAsync()).Select(fig => fig.AsFigure()).ToList();
            Board board = new Board(figuresOnBoard);

            FieldEntity? enPassant = await enPassantRepository.GetEnPassantFieldAsync();
            if (enPassant != null) board.EnPassantField = enPassant.AsVector2();

            Figure figureInspected = (await figureRepository.GetFigureAsync(id)).AsFigure();
            if (figureInspected is null)
            {
                return NotFound();
            }
            List<Vector2> allowedMoves = chessValidator.GetAllowedMoves(board, figureInspected);

            return allowedMoves.Select(vec => vec.AsFieldDto()).ToList();
        }

    }
}
