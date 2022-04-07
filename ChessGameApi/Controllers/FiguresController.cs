#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChessGameApi.Models;

namespace ChessGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiguresController : ControllerBase
    {
        private readonly FigureContext _context;

        public FiguresController(FigureContext context)
        {
            _context = context;
        }

        // GET: api/Figures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Figure>>> GetFigures()
        {
            return await _context.Figures.ToListAsync();
        }

        // GET: api/Figures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Figure>> GetFigure(long id)
        {
            var figure = await _context.Figures.FindAsync(id);

            if (figure == null)
            {
                return NotFound();
            }

            return figure;
        }

        // PUT: api/Figures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFigure(long id, Figure figure)
        {
            if (id != figure.Id)
            {
                return BadRequest();
            }

            _context.Entry(figure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FigureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Figures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Figure>> PostFigure(Figure figure)
        {
            _context.Figures.Add(figure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFigure", new { id = figure.Id }, figure);
        }

        // DELETE: api/Figures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFigure(long id)
        {
            var figure = await _context.Figures.FindAsync(id);
            if (figure == null)
            {
                return NotFound();
            }
             
            _context.Figures.Remove(figure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FigureExists(long id)
        {
            return _context.Figures.Any(e => e.Id == id);
        }
    }
}
