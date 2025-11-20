namespace Proyecto.Core.Entidades;

public class Orden
{
    public int idOrden { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public int idCliente { get; set; }
    public int NumeroOrden { get; set; }

    public int idTarifa { get; set; }
    public Cliente Cliente { get; set; } = default!;
    public List<Entrada> Entradas { get; set; } = new List<Entrada>();
    public decimal Total { get; set; }
    public List<DetalleOrden>? Detalles { get; set; }
}