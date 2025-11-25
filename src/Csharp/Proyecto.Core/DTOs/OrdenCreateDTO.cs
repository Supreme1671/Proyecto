namespace Proyecto.Core.DTOs;

public class OrdenCreateDTO
{
    public int IdCliente { get; set; }
    public List<int> idFuncion { get; set; } = new();
    public List<int> idTarifa { get; set; } = new();
    public List<int> Cantidades { get; set; } = new();
}