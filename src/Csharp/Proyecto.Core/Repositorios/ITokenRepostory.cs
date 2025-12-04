using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;
namespace Proyecto.Core.Repositorios;
public interface ITokenRepostory
{
    int InsertarToken(Token token);
    Token? ObtenerToken(string token);
    void EliminarToken(string token);
    void EliminarTokensPorEmail(string email);
    void ReemplazarToken(int IdUsuario, string nuevoHash, DateTime fechaExpiracion);
}
