using ChessGameApi.Dtos;
using ChessGameApi.Exceptions;
using ChessGameApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("LocalAngular")]
    public class ChessController : ControllerBase
    {
        private readonly IBoardService boardService;

        public ChessController(IBoardService boardService)
        {
            this.boardService = boardService;
        }

        //GET /chess/figures
        [HttpGet("figures")]
        public async Task<ActionResult<IEnumerable<FigureDto>>> GetFiguresAsync()
        {
            List<FigureDto> figures = await boardService.GetFiguresAsync();
            return Ok(figures);
        }

        //GET /chess/figures/available
        [HttpGet("figures/available")]
        public ActionResult<IEnumerable<NewFigureDto>> GetNewFiguresAsync()
        {
            List<NewFigureDto> figures = boardService.GetNewFigures();
            return Ok(figures);
        }

        //GET /chess/figures/{id}
        [HttpGet("figures{id}")]
        public async Task<ActionResult<FigureDto>> GetFigureAsync(Guid id)
        {
            var figure = await boardService.GetFigureAsync(id);
            if (figure is null)
            {
                return NotFound($"Figure with id={id} has not been found.");
            }
            return figure;
        }

        // POST /chess/figures
        [HttpPost("figures")]
        public async Task<ActionResult<IEnumerable<FigureDto>>> CreateFigureAsync(CreateFigureDto newFigureDto)
        {
            try
            {
                await boardService.PutNewFigureAsync(newFigureDto);
            } catch(FieldOccupiedException e)
            {
                return BadRequest(e);
            }
            return await GetFiguresAsync();
        }

        // PUT /chess/figures/{id}
        [HttpPut("figures/{id}")]
        public async Task<ActionResult<IEnumerable<FigureDto>>> MoveFigureAsync(Guid id, FieldDto newPositionDto)
        {
            try
            {
                await boardService.MoveFigureAsync(id, newPositionDto);
                return GetFiguresAsync().Result;
            }
            catch (FigureNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (FieldOccupiedException e)
            {
                return BadRequest(e.Message);
            }
            catch (MoveNotAllowedException e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE /Chess/figures/{id}
        [HttpDelete("figures/{id}")]
        public async Task<ActionResult> DeleteFigureAsync(Guid id)
        {
            try
            {
                await boardService.DeleteFigureAsync(id);
            }
            catch (FigureNotFoundException e)
            {
                return NotFound(e);
            }

            return NoContent();
        }

        [HttpGet("getAllowedMoves/{id}")]
        public async Task<ActionResult<List<FieldDto>>> GetAllowedMoves(Guid id)
        {
            try
            {
                return await boardService.GetAllowedMoves(id);
            } catch (ChessApiException e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
