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
        public DbSet<Usuario> Usuarios { get; set; }  // EF Core crea la tabla Usuarios
    }
}
