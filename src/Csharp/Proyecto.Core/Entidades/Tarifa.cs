namespace Proyecto.Core.Entidades;

public class Tarifa
{
    public int idTarifa { get; set; }
    public decimal Precio { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int idSector { get; set; }
    public int idFuncion { get; set; }
    public int idEvento { get; set; }

    public int Stock { get; set; }  // NUEVO
    public bool Activa { get; set; } // NUEVO

    public Sector Sector { get; set; } = default!;
    public Funcion Funcion { get; set; } = default!;
}
