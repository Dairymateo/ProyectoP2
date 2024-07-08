
using ProyectoP2.Models;
using ProyectoP2.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ProyectoP2.DataAccess
{
    public class VentaDbContext : DbContext
    {
        public VentaDbContext()
        {
        }

        public VentaDbContext(DbContextOptions<VentaDbContext> options) : base(options) { }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=VentaDatabase.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.IdCategoria);
                entity.Property(c => c.IdCategoria).IsRequired().ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(p => p.IdProducto);
                entity.Property(p => p.IdProducto).IsRequired().ValueGeneratedOnAdd();
                entity.HasOne(p => p.RefCategoria).WithMany(c => c.Productos)
                .HasForeignKey(p => p.IdCategoria);
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(v => v.IdVenta);
                entity.Property(v => v.IdVenta).IsRequired().ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(dv => dv.IdDetalleVenta);
                entity.Property(dv => dv.IdDetalleVenta).IsRequired().ValueGeneratedOnAdd();
                entity.HasOne(dv => dv.RefVenta).WithMany(v => v.RefDetalleVenta)
                .HasForeignKey(dv => dv.IdVenta);
                entity.HasOne(dv => dv.RefProducto).WithMany(p => p.RefDetalleVenta)
                .HasForeignKey(dv => dv.IdProducto);
            });

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Nombre = "Ficción" },
                new Categoria { IdCategoria = 2, Nombre = "No Ficción" },
                new Categoria { IdCategoria = 3, Nombre = "Infantil y Juvenil" },
                new Categoria { IdCategoria = 4, Nombre = "Arte y Cultura" },
                new Categoria { IdCategoria = 5, Nombre = "Ciencia y Naturaleza" },
                new Categoria { IdCategoria = 6, Nombre = "Religión y Espiritualidad" }
            );

            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    IdProducto = 1,
                    Codigo = "9780061122415",
                    Nombre = "To Kill a Mockingbird",
                    IdCategoria = 1,
                    Cantidad = 20,
                    Precio = 12
                },
                new Producto
                {
                    IdProducto = 2,
                    Codigo = "9781982105402",
                    Nombre = "Educated: A Memoir",
                    IdCategoria = 2,
                    Cantidad = 30,
                    Precio = 15
                },
                new Producto
                {
                    IdProducto = 3,
                    Codigo = "9780439023481",
                    Nombre = "Harry Potter and the Sorcerer's Stone",
                    IdCategoria = 3,
                    Cantidad = 30,
                    Precio = 9
                },
                new Producto
                {
                    IdProducto = 4,
                    Codigo = "9780714878633",
                    Nombre = "The Art Book",
                    IdCategoria = 4,
                    Cantidad = 25,
                    Precio = 39
                },
                new Producto
                {
                    IdProducto = 5,
                    Codigo = "9781426217174",
                    Nombre = "National Geographic Kids Almanac 2023",
                    IdCategoria = 5,
                    Cantidad = 15,
                    Precio = 14
                },
                new Producto
                {
                    IdProducto = 6,
                    Codigo = "9780141439471",
                    Nombre = "Mere Christianity",
                    IdCategoria = 6,
                    Cantidad = 18,
                    Precio = 8
                }
            );

        }
    }
}
