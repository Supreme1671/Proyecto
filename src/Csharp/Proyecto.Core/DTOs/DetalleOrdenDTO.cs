namespace Proyecto.Core.DTOs;

public class DetalleOrdenDTO
{
    public int IdDetalleOrden { get; set; }
    public int idFuncion { get; set; }
    public int idEvento { get; set; }
    public int idTarifa { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
