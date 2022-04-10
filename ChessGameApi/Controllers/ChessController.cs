using ChessGameApi.Dtos;
using ChessGameApi.Entities;
using ChessGameApi.Extensions;
using ChessGameApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChessController : ControllerBase
    {
        private readonly IFigureRepository figureRepository;

        public ChessController(IFigureRepository figureRepository)
        {
            this.figureRepository = figureRepository;
        }

        //GET /chess
        [HttpGet]
        public IEnumerable<FigureDto> GetFigures()
        {
            var figures = figureRepository.GetFigures().Select(fig => fig.AsDto());
            return figures;
        }

        //GET /chess/{id}
        [HttpGet("{id}")]
        public ActionResult<FigureDto> GetFigure(Guid id)
        {
            var figure = figureRepository.GetFigure(id);
            if (figure is null)
            {
                return NotFound();
            }
            return figure.AsDto();
        }

        // POST /chess
        [HttpPost]
        public ActionResult<FigureDto> CreateFigure(CreateFigureDto newFigureDto)
        {
            FigureEntity figureEntity = new()
            {
                Id = Guid.NewGuid(),
                X = newFigureDto.X,
                Y = newFigureDto.Y,
                Player = newFigureDto.Player,
                FigureType = newFigureDto.FigureType
            };

            figureRepository.CreateFigure(figureEntity);
            return CreatedAtAction(nameof(GetFigure), new { id = figureEntity.Id }, figureEntity.AsDto());
        }

        // PUT /chess/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateFigure(Guid id, UpdateFigureDto figureDto)
        {
            var existingFigure = figureRepository.GetFigure(id);
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

            figureRepository.UpdateFigure(updatedFigure);
            return NoContent();
        }

        // DELETE /Chess/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteFigure(Guid id)
        {
            var existingFigure = figureRepository.GetFigure(id);
            if (existingFigure is null)
            {
                return NotFound();
            }

            figureRepository.DeleteFigure(id);
            return NoContent();
        }
        
    }
}
