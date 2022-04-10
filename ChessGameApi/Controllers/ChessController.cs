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
        public ActionResult<FigureDto> GetFigure(long id)
        {
            var figure = figureRepository.GetFigure(id);
            if(figure is null)
            {
                return NotFound();
            }
            return figure.AsDto();
        }

        
    }
}
