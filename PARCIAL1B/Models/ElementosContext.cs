using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace PARCIAL1B.Models
{
    public class ElementosContext : DbContext
    {
        public ElementosContext(DbContextOptions<ElementosContext> options) : base(options)
        {

        }

        public DbSet<ElementosPorPlato> ElementosPorPlatos { get; set; }
        public DbSet<Elementos> Elementos { get; set; }
        public DbSet<Platos> Platos { get; set; }
        public DbSet<PlatosPorCombo> PlatosPorCombos { get; set; }
    }
}
