using Microsoft.EntityFrameworkCore;

namespace StarChart.Data
{
    using StarChart.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CelestialObject> CelestialObjects { get; set; }
    }
}
