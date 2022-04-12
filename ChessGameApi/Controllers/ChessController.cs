﻿using ChessGameApi.Dtos;
using ChessGameApi.Entities;
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


            await figureRepository.CreateFigureAsync(figureEntity);
            return CreatedAtAction(nameof(GetFigureAsync), new { id = figureEntity.Id }, figureEntity.AsDto());
        }

        // PUT /chess/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> MoveFigureAsync(Guid id, MoveFigureDto desiredFigureDto)
        {
            var existingFigure = await figureRepository.GetFigureAsync(id);
            if(existingFigure is null)
            {
                return NotFound($"Figure with id: {id} has not been found.");
            }
            
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

            //make move
            FigureEntity afterMove = existingFigure with
            {
                X = desiredFigureDto.X,
                Y = desiredFigureDto.Y,
            };

            await figureRepository.UpdateFigureAsync(afterMove);
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

            // check for enPassantField
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
