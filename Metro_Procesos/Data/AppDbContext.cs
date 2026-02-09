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

        // Definición de las tablas (DbSets)
        public DbSet<Usuario> usuario { get; set; }
        public DbSet<Tarjeta> tarjeta { get; set; }
        public DbSet<CuentaBancaria> cuenta_bancaria { get; set; }
        public DbSet<Recarga> recarga { get; set; }
        public DbSet<Boleto> boleto { get; set; }

        // Configuración de mapeo explícito para SQL Server
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fuerza a C# a usar los nombres de tabla en minúsculas y con guiones bajos
            modelBuilder.Entity<Usuario>().ToTable("usuario");
            modelBuilder.Entity<Tarjeta>().ToTable("tarjeta");
            modelBuilder.Entity<CuentaBancaria>().ToTable("cuenta_bancaria");
            modelBuilder.Entity<Recarga>().ToTable("recarga");
            modelBuilder.Entity<Boleto>().ToTable("boleto");

            // Configuración de precisión para el saldo de dinero
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Saldo)
                .HasColumnType("decimal(10,2)");
        }
    }
}