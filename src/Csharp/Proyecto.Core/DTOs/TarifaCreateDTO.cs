namespace Proyecto.Core.DTOs;

public class TarifaCreateDTO
{
    public decimal Precio { get; set; }
    public string Nombre { get; set; } = "";
    public int idSector { get; set; }
    public int idFuncion { get; set; }
    public int idEvento { get; set; }
    public int Stock { get; set; }   
}
