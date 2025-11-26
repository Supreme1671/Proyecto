using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;
using Proyecto.Core.Repositorios;


namespace Repositorios.Repos
{
    public class SectorRepository : ISectorRepository
    {
        private readonly string _connectionString;

        public SectorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
        }

        private MySqlConnection Connection => new MySqlConnection(_connectionString);

        // GET /locales/{localId}/sectores
        public IEnumerable<Sector> GetByLocal(int idLocal)
        {
            using var db = Connection;

            var sql = "SELECT * FROM Sector WHERE IdLocal = @idLocal";

            return db.Query<Sector>(sql, new { idLocal });
        }

        // POST /locales/{localId}/sectores
        public void Add(int idLocal, Sector sector)
        {
            using var db = Connection;

            var sql = @"
                INSERT INTO Sector (Nombre, IdLocal, Capacidad, Precio)
                VALUES (@Nombre, @IdLocal, @Capacidad, @Precio);
                SELECT LAST_INSERT_ID();
            ";

            sector.idLocal = idLocal;
            sector.idSector = db.ExecuteScalar<int>(sql, sector);
        }

        // GET interno
        public Sector? GetById(int idSector)
        {
            using var db = Connection;

            var sql = "SELECT * FROM Sector WHERE IdSector = @idSector";

            return db.QueryFirstOrDefault<Sector>(sql, new { idSector });
        }

        // PUT /sectores/{sectorId}
        public bool Update(int idSector, SectorUpdateDTO dto)
        {
            using var db = Connection;

            var sql = @"
                UPDATE Sector
                SET Nombre = @Nombre,
                    Capacidad = @Capacidad,
                    Precio = @Precio
                WHERE IdSector = @IdSector
            ";

            int rows = db.Execute(sql, new
            {
                dto.Nombre,
                dto.Capacidad,
                dto.Precio,
                IdSector = idSector
            });

            return rows > 0;
        }

        // DELETE /sectores/{sectorId}
        public bool Delete(int idSector)
        {
            using var db = Connection;

            var sql = "DELETE FROM Sector WHERE IdSector = @idSector";

            return db.Execute(sql, new { idSector }) > 0;
        }
    }
}
