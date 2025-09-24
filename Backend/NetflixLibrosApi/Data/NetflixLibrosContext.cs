using Microsoft.EntityFrameworkCore;
using NetflixLibrosAPI.Modelos;

namespace NetflixLibrosAPI.Data
{
    public class NetflixLibrosContext : DbContext
    {
        public NetflixLibrosContext(DbContextOptions<NetflixLibrosContext> options)
            : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.ToTable("Libros");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Titulo)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Autor)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.UrlPortada)
                      .HasMaxLength(500);

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(1000);

                entity.Property(e => e.UrlPdf)
                      .HasMaxLength(500);
            });
        }
    }
}
