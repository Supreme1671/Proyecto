namespace Proyecto.Core.DTOs;
public class OrdenDTO
{
    public int IdOrden { get; set; }
    public int IdCliente { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; }

    public List<DetalleOrdenDTO> Detalles { get; set; } = new();
}
