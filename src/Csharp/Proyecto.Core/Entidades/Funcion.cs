namespace Proyecto.Core.Entidades;

public class Funcion
{
    public int idFuncion { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public int idEvento { get; set; }
    public int idLocal { get; set; }
    public int Activo { get; set; } = 1;
    public List<Entrada> Entradas { get; set; } = new();
}
