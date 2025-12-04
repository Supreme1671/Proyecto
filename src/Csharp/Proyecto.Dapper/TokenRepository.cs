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
    INSERT INTO Token (IdUsuario, TokenRefresh, TokenHash, Email, FechaExpiracion)
    VALUES (@IdUsuario, @TokenRefresh, @TokenHash, @Email, @FechaExpiracion);
    SELECT LAST_INSERT_ID();
";


    return connection.QuerySingle<int>(sql, token);
}

    public Token? ObtenerToken(string token)
{
    using var connection = new MySqlConnection(_connectionString);

    string sql = @"
        SELECT * FROM Token 
        WHERE TokenRefresh = @Token;
    ";

    return connection.QueryFirstOrDefault<Token>(sql, new { Token = token });
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
        FechaExpiracion = @FechaExpiracion
    WHERE Email = (SELECT Email FROM Usuario WHERE IdUsuario = @IdUsuario)
";

connection.Execute(sql, new
{
    NuevoHash = nuevoHash,
    FechaExpiracion = expiracion,
    idUsuario = IdUsuario
});

    }
}
