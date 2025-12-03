using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.Entidades;
using Proyecto.Core.Repositorios;

namespace Proyecto.Dapper;

public class TokenRepository : ITokenRepostory
{
    private readonly string _connectionString;

    public TokenRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection")!;
    }

    public int InsertarToken(Token token)
    {
        using var connection = new MySqlConnection(_connectionString);

        string sql = @"
            INSERT INTO Token (TokenHash, Email, Expiracion)
            VALUES (@TokenHash, @Email, @Expiracion);
            SELECT LAST_INSERT_ID();
        ";

        return connection.QuerySingle<int>(sql, token);
    }

    public Token? ObtenerToken(string token)
    {
        using var connection = new MySqlConnection(_connectionString);

        string sql = @"
            SELECT * FROM Token 
            WHERE TokenHash = @TokenHash;
        ";

        return connection.QueryFirstOrDefault<Token>(sql, new { TokenHash = token });
    }

    public void EliminarToken(string token)
    {
        using var connection = new MySqlConnection(_connectionString);

        string sql = @"DELETE FROM Token WHERE TokenHash = @TokenHash;";

        connection.Execute(sql, new { TokenHash = token });
    }

    public void EliminarTokensPorEmail(string email)
    {
        using var connection = new MySqlConnection(_connectionString);

        string sql = @"DELETE FROM Token WHERE Email = @Email;";

        connection.Execute(sql, new { Email = email });
    }

    public void ReemplazarToken(int IdUsuario, string nuevoHash, DateTime expiracion)
    {
        using var connection = new MySqlConnection(_connectionString);

        string sql = @"
            UPDATE Token
            SET TokenHash = @NuevoHash,
                Expiracion = @Expiracion
            WHERE Email = (SELECT Email FROM Usuario WHERE IdUsuario = @IdUsuario)
        ";

        connection.Execute(sql, new
        {
            NuevoHash = nuevoHash,
            Expiracion = expiracion,
            IdUsuario = IdUsuario
        });
    }
}
