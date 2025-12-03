using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;
namespace Proyecto.Core.Repositorios;
public interface ITokenRepostory
{
    public int InsertarToken(Token token);
        public Token? ObtenerToken(string token);
        public void EliminarToken(string token);
        public void EliminarTokensPorEmail(string email);
        public void ReemplazarToken(int IdUsuario, string nuevoHash, DateTime expiracion);
}
