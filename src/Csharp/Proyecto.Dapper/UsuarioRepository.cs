using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios.ReposDapper;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public bool ExisteUsuario(string email)
    {
        using var db = Connection;
        const string sql = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email";
        int cantidad = db.ExecuteScalar<int>(sql, new { Email = email });
        return cantidad > 0;
    }
    public void InsertarUsuario(Usuario usuario)
    {
        using var db = Connection;
        const string sql = @"
            INSERT INTO Usuario (NombreUsuario, Email, Contrasena, Roles, Activo)
            VALUES (@NombreUsuario, @Email, @Contrasena, @Roles, @Activo)
        ";
        db.Execute(sql, usuario);
    }
    public Usuario? ObtenerUsuarioPorEmail(string email)
    {
        using var db = Connection;
        const string sql = "SELECT * FROM Usuario WHERE Email = @Email";
        return db.QueryFirstOrDefault<Usuario>(sql, new { Email = email });
    }
    public Usuario? ObtenerUsuarioPorId(int idUsuario)
    {
        using var db = Connection;
        const string sql = "SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario";
        return db.QueryFirstOrDefault<Usuario>(sql, new { IdUsuario = idUsuario });
    }
    public void ActualizarRol(int idUsuario, string nuevoRol)
    {
        using var db = Connection;
        const string sql = "UPDATE Usuario SET Roles = @Rol WHERE IdUsuario = @IdUsuario";
        db.Execute(sql, new { Rol = nuevoRol, IdUsuario = idUsuario });
    }
    public IEnumerable<string> ObtenerTodosLosRoles()
    {
        using var db = Connection;
        const string sql = "SELECT Nombre FROM Rol";
        return db.Query<string>(sql);
    }
}
