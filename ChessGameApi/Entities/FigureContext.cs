using Microsoft.EntityFrameworkCore;

namespace ChessGameApi.Entities
{
    public class FigureContext : DbContext
    {
        public FigureContext(DbContextOptions<FigureContext> options) : base(options)
        {
        }

        public DbSet<FigureEntity> Figures { get; set; } = null!;
    }
}
