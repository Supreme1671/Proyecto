public class Entrada
{
    public int IdEntrada { get; set; }
    public decimal Precio { get; set; }
    public string QR { get; set; } = string.Empty;
    public bool Usada { get; set; }
    public bool Anulada { get; set; }
    public required string Numero { get; set; }

    public int? IdDetalleOrden { get; set; }
    public int? IdSector { get; set; }   
    public int? IdFuncion { get; set; }
    public int? IdCliente { get; set; }
    public int? IdTarifa { get; set; }

    public string Estado { get; set; } = "Disponible";
}
