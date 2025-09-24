using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixLibrosAPI.Data;
using NetflixLibrosAPI.Modelos;

namespace NetflixLibrosAPI.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly NetflixLibrosContext _context;

        public LibrosController(NetflixLibrosContext context)
        {
            _context = context;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> ObtenerLibros()
        {
            return await _context.Libros.ToListAsync();
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> ObtenerLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return libro;
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<Libro>> CrearLibro(Libro libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerLibro), new { id = libro.Id }, libro);
        }
    }
}
