using ChessGameApi.Dtos;
using ChessGameApi.Entities;
using ChessGameApi.Exceptions;
using ChessGameApi.Extensions;
using ChessGameApi.Repositories;
using ChessGameSketch;
using ChessGameSketch.Validator;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace ChessGameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("LocalAngular")]
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
        public async Task<ActionResult<IEnumerable<FigureDto>>> GetFiguresAsync()
        {
            var figures = (await figureRepository.GetFiguresAsync())
                .Select(fig => fig.AsDto());
            return Ok(figures);
        }

        //GET /chess/available
        [HttpGet("available")]
        public ActionResult<IEnumerable<FigureDto>> GetNewFiguresAsync()
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

            return Ok(figures);
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
        public async Task<ActionResult<IEnumerable<FigureDto>>> CreateFigureAsync(CreateFigureDto newFigureDto)
        {
            if(await figureRepository.HasFigureAtAsync(newFigureDto.X, newFigureDto.Y)) {
                return BadRequest("Selected position is occupied by another figure.");
            }

            FigureEntity figureEntity = new()
            {
                Id = Guid.NewGuid(),
                X = newFigureDto.X,
                Y = newFigureDto.Y,
                Player = newFigureDto.Player,
                FigureType = newFigureDto.FigureType
            };

            await figureRepository.CreateFigureAsync(figureEntity);
            return await GetFiguresAsync();
        }

        // PUT /chess/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<FigureDto>>> MoveFigureAsync(Guid id, MoveFigureDto desiredFigureDto)
        {
            FigureEntity existingFigure;
            try
            {
                existingFigure = await figureRepository.GetFigureAsync(id);
            }
            catch (ChessApiException e)
            {
                return NotFound(e.Message);
            }

            // validate move

            List<Vector2> allowedMoves;
            try
            {
                allowedMoves = await FindAllowedMoves(id);
            }
            catch (ChessApiException e)
            {
                return BadRequest(e.Message);
            }

            Vector2 desiredPosition = new Vector2(desiredFigureDto.X, desiredFigureDto.Y);
            if (allowedMoves.Contains(desiredPosition))
            {
                //handle en passant
                await enPassantRepository.ClearEnPassantFieldAsync();
                if (existingFigure.FigureType == "Pawn")
                {
                    if (Math.Abs(existingFigure.Y - desiredFigureDto.Y) == 2)
                    {
                        FieldEntity enPassantField = new()
                        {
                            Id = Guid.NewGuid(),
                            X = existingFigure.X,
                            Y = (existingFigure.Y + desiredFigureDto.Y) / 2
                        };
                        await enPassantRepository.NewEnPassantFieldAsync(enPassantField);
                    }
                }

                //remove figure on desired position, if any
                try
                {
                    await figureRepository.DeleteFigureAtPositionAsync((int) desiredPosition.X, (int) desiredPosition.Y);
                } catch (FigureNotFoundException)
                { }

                //make move
                FigureEntity afterMove = existingFigure with
                {
                    X = desiredFigureDto.X,
                    Y = desiredFigureDto.Y,
                };

                await figureRepository.UpdateFigureAsync(afterMove);

                return GetFiguresAsync().Result;
            }
            else
            {
                return Problem(
                            type: "/docs/errors/forbidden",
                            title: "Move is forbidden.",
                            detail: $"Figure can not move to desired field.",
                            statusCode: StatusCodes.Status403Forbidden,
                            instance: HttpContext.Request.Path
                       );
            }
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
            try
            {
                List<Vector2> allowedMoves = await FindAllowedMoves(id);
                return allowedMoves.Select(vec => vec.AsFieldDto()).ToList();
            } catch (ChessApiException e)
            {
                return BadRequest(e.Message);
            }
        }

        private async Task<List<Vector2>> FindAllowedMoves(Guid id)
        {
            List<Figure> figuresOnBoard = (await figureRepository.GetFiguresAsync()).Select(fig => fig.AsFigure()).ToList();
            Board board = new Board(figuresOnBoard);

            // check for enPassantField
            FieldEntity? enPassant = await enPassantRepository.GetEnPassantFieldAsync();

            if (enPassant != null) board.EnPassantField = enPassant.AsVector2();

            Figure figureInspected;
            try
            {
                figureInspected = (await figureRepository.GetFigureAsync(id)).AsFigure();
            }
            catch (ChessApiException)
            {
                throw;
            }

            List<Vector2> allowedMoves = chessValidator.GetAllowedMoves(board, figureInspected);
            return allowedMoves;


        }

    }
}
