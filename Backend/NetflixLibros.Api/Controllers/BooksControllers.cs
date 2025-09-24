using Microsoft.AspNetCore.Mvc;
using NetflixLibrosAPI.Modelos;

namespace NetflixLibrosAPI.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private static List<Libro> Libros = new List<Libro>
        {
            new Libro { Id = 1, Titulo = "El Principito", Autor = "Antoine de Saint-Exupéry", UrlPortada="/portadas/el-principito.jpeg", Descripcion="Un cuento maravilloso..." },
            new Libro { Id = 2, Titulo = "20 Mil Leguas de Viaje Submarino", Autor = "Julio Verne", UrlPortada="/portadas/20mil.jpeg", Descripcion="Aventura submarina..." }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Libro>> ObtenerLibros() => Ok(Libros);

        [HttpGet("{id}")]
        public ActionResult<Libro> ObtenerLibro(int id)
        {
            var libro = Libros.FirstOrDefault(l => l.Id == id);
            return libro == null ? NotFound() : Ok(libro);
        }
    }
}
