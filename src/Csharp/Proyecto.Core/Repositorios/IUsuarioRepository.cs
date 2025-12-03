using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios
{
    public interface IUsuarioRepository
    {
        bool ExisteUsuario(string email);
        void InsertarUsuario(Usuario usuario);
        Usuario? ObtenerUsuarioPorEmail(string email);
        Usuario? ObtenerUsuarioPorId(int idUsuario);
        void ActualizarRol(int idUsuario, string nuevoRol);
        IEnumerable<string> ObtenerTodosLosRoles();
    }
}
