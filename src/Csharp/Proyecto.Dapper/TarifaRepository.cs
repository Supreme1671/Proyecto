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
        INSERT INTO Tarifa (Precio, Descripcion, idSector, idFuncion, idEvento)
        VALUES (@Precio, @Descripcion, @idSector, @idFuncion, @idEvento);
    ";

    connection.Execute(sql, new
    {
        Precio = tarifa.Precio,
        Descripcion = tarifa.Nombre,
        IdSector = tarifa.idSector,
        IdFuncion = tarifa.idFuncion,
        IdEvento = tarifa.idEvento
    });
}
void ITarifaRepository.Add(Tarifa tarifa)
{
    Add(tarifa);
}



 public IEnumerable<Tarifa> GetByFuncionId(int idFuncion)
{
    using var db = Connection;
    string sql = "SELECT * FROM Tarifa WHERE IdFuncion = @idFuncion;";
    return db.Query<Tarifa>(sql, new { idFuncion });
}

    public Tarifa GetById(int idTarifa)
    {
        using var db = Connection;
        string sql = "SELECT * FROM Tarifa WHERE idTarifa = @idTarifa;";
        return db.QueryFirstOrDefault<Tarifa>(sql, new { idTarifa });
    }

    public void Update(Tarifa tarifa)
{
    using var db = Connection;
    string sql = @"
        UPDATE Tarifa
        SET Precio = @Precio,
            Stock = @Stock,
            Activa = @Activa
        WHERE idTarifa = @idTarifa;
    ";
    db.Execute(sql, tarifa);
}

    public bool Update(int idTarifa, TarifaUpdateDTO dto)
    {
        throw new NotImplementedException();
    }


}
