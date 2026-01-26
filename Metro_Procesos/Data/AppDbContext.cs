using Microsoft.EntityFrameworkCore;
using Metro_Procesos.Models;

namespace Metro_Procesos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

    }
}

