using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;

namespace Proyecto.Core.Repositorios.ReposDapper;
public class SectorRepository : ISectorRepository
{
    private readonly string _connectionString;

    public SectorRepository(IConfiguration configuration)
    {   
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public void Add(Sector sector)
    {
        using var db = Connection;
        string sql = @"
            INSERT INTO Sector (Nombre, IdLocal, Capacidad, Precio)
            VALUES (@Nombre, @IdLocal, @Capacidad, @Precio);";
        
        db.Execute(sql, sector);
    }

    public IEnumerable<Sector> GetAll()
    {
        using var db = Connection;
        string sql = "SELECT * FROM Sector;";
        return db.Query<Sector>(sql);
    }

    public Sector? GetById(int idSector)
    {
        using var db = Connection;
        string sql = "SELECT * FROM Sector WHERE idSector = @Id;";
        return db.QueryFirstOrDefault<Sector>(sql, new { Id = idSector });
    }

    public void Update(Sector sector)
    {
        using var db = Connection;
        string sql = @"
            UPDATE Sector 
            SET Nombre = @Nombre, Capacidad = @Capacidad, Precio = @Precio
            WHERE idSector = @idSector;";
        
        db.Execute(sql, sector);
    }

    public void Delete(int idSector)
    {
        using var db = Connection;
        string sql = "DELETE FROM Sector WHERE idSector = @Id;";
        db.Execute(sql, new { Id = idSector });
    }

    public void Add(int idLocal, Sector sector)
    {
        using var db = Connection;
        string sql = @"
            INSERT INTO Sector (Nombre, IdLocal)
            VALUES (@Nombre, @IdLocal);";
        
        db.Execute(sql, new { sector.Nombre, IdLocal = idLocal });
    }

    public bool Update(int idSector, SectorDTO dto)
    {
        using var db = Connection;

        string sql = @"
            UPDATE Sector 
            SET Nombre = @Nombre, Capacidad = @Capacidad, Precio = @Precio
            WHERE idSector = @Id;";

        int res = db.Execute(sql, new
        {
            dto.Nombre,
            dto.Capacidad,
            dto.Precio,
            Id = idSector
        });

        return res > 0;
    }

    bool ISectorRepository.Delete(int idSector)
    {
        using var db = Connection;
        string sql = "DELETE FROM Sector WHERE idSector = @Id;";
        int res = db.Execute(sql, new { Id = idSector });
        return res > 0;
    }
}
