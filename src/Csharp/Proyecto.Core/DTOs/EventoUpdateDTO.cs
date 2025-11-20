namespace Proyecto.Core.DTOs;

public class EventoUpdateDTO
{
public string Nombre { get; set; }
public DateTime Fecha { get; set; }
public string Tipo { get; set; }
public string Descripcion { get; set; }
public int idLocal { get; set; }
public bool Activo { get; set; }
}
