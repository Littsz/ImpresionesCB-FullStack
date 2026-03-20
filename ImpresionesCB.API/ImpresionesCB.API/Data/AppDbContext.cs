using Microsoft.EntityFrameworkCore;
using ImpresionesCB.API.Models;

namespace ImpresionesCB.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ConfiguracionEtiqueta> Configuracion { get; set; }
    }
}
