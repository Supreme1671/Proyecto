namespace Proyecto.Core.Entidades;

public class Token
{
    public int IdToken { get; set; }
    public int IdUsuario { get; set; }
    public string TokenRefresh { get; set; } = string.Empty;
    public string TokenHash { get; set; } = "";
    public string Email { get; set; } = string.Empty;
    public DateTime FechaExpiracion { get; set; }
}