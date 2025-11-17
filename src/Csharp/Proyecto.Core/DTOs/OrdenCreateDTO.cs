namespace Proyecto.Core.DTOs;

public class OrdenCreateDTO
{
    public int IdCliente { get; set; }
    public List<int> IdFunciones { get; set; } = new();
    public List<int> IdTarifas { get; set; } = new();
    public List<int> Cantidades { get; set; } = new();
}