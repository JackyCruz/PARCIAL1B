using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace PARCIAL1B.Models
{
    public class ElementosContext : DbContext
    {
        public ElementosContext(DbContextOptions<ElementosContext> options) : base(options)
        {

        }

        public DbSet<ElementosPorPlato> ElementosPorPlato { get; set; }
        public DbSet<Elementos> Elementos { get; set; }
        public DbSet<Platos> Platos { get; set; }
        public DbSet<PlatosPorCombo> PlatosPorCombo { get; set; }
    }
}
