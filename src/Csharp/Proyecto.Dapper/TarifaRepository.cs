using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios.ReposDapper;

public class TarifaRepository : ITarifaRepository
{
    private readonly string _connectionString;

    public TarifaRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public void Add(Tarifa tarifa)
{
    using var connection = new MySqlConnection(_connectionString);

    var sql = @"
        INSERT INTO Tarifa 
        (Precio, Descripcion, Stock, Activo, idSector, idFuncion, idEvento)
        VALUES 
        (@Precio, @Descripcion, @Stock, @Activo, @idSector, @idFuncion, @idEvento);
        SELECT LAST_INSERT_ID();
    ";

    tarifa.idTarifa = connection.ExecuteScalar<int>(sql, tarifa);
}

    public bool RestarStock(int idTarifa, int cantidad)
{
    using var db = Connection;

    string sql = @"
        UPDATE Tarifa
        SET Stock = Stock - @cantidad
        WHERE idTarifa = @idTarifa AND Stock >= @cantidad;
    ";

    int filas = db.Execute(sql, new { idTarifa, cantidad });

    return filas > 0; 
}
    public IEnumerable<Tarifa> GetByFuncionId(int idFuncion)
{
    using var db = Connection;

    string sql = @"
        SELECT 
            idTarifa,
            Precio,
            Descripcion,
            Stock,
            Activo,
            idSector,
            idFuncion,
            idEvento
        FROM Tarifa
        WHERE idFuncion = @idFuncion;";

    return db.Query<Tarifa>(sql, new { idFuncion });
}


   public Tarifa GetById(int idTarifa)
{
    using var db = Connection;

    string sql = @"
        SELECT 
            idTarifa,
            Precio,
            Descripcion,
            Stock,
            Activo,
            idSector,
            idFuncion,
            idEvento
        FROM Tarifa
        WHERE idTarifa = @idTarifa;
    ";

    return db.QueryFirstOrDefault<Tarifa>(sql, new { idTarifa });
}


    public bool Update(int idTarifa, TarifaUpdateDTO dto)
{
    using var db = Connection;

    string sql = @"
        UPDATE Tarifa
        SET Precio = @Precio,
            Stock = @Stock,
            Activo = @Activo
        WHERE idTarifa = @idTarifa;
    ";

    int rows = db.Execute(sql, new
    {
        idTarifa,
        dto.Precio,
        dto.Stock,
        dto.Activo
    });

    return rows > 0;
}


}
