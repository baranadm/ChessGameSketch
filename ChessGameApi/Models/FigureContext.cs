using Microsoft.EntityFrameworkCore;

namespace ChessGameApi.Models
{
    public class FigureContext : DbContext
    {
        public FigureContext(DbContextOptions<FigureContext> options) : base(options)
        {
        }

        public DbSet<Figure> Figures { get; set; } = null!;
    }
}
