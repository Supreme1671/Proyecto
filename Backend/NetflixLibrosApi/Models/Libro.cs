namespace NetflixLibrosAPI.Modelos
{
    public class Libro
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Autor { get; set; } = string.Empty;

        public string? UrlPortada { get; set; }

        public string? Descripcion { get; set; }

        // Nueva propiedad para la ruta o URL del PDF
        public string? UrlPdf { get; set; }
    }
}
