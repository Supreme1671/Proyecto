namespace Proyecto.Core.DTOs;

public class QrDTO
{
    public int idQR { get; set; }
    public int IdEntrada { get; set; }
    public string Codigo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string ImagenBase64 { get; set; }
    public string Estado {get; set;}
}