using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios.ReposDapper
{
    public class EventoRepository : IEventoRepository
    {
        private readonly string _connectionString;

        public EventoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public IEnumerable<Evento> GetAll()
        {
            using var db = Connection;
            return db.Query<Evento>("SELECT * FROM Evento");
        }

        public Evento? GetById(int idEvento)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Evento>(
                "SELECT * FROM Evento WHERE idEvento = @Id",
                new { Id = idEvento });
        }

        public int Add(Evento evento)
        {
            using var db = Connection;

            string sql = @"
                INSERT INTO Evento (Nombre, Fecha, Activo, idLocal)
                VALUES (@Nombre, @Fecha, @Activo, @idLocal);
                SELECT LAST_INSERT_ID();
            ";

            return db.ExecuteScalar<int>(sql, evento);
        }

        public bool Update(int idEvento, EventoUpdateDTO dto)
        {
            using var db = Connection;

            string sql = @"
                UPDATE Evento SET
                    Nombre = @Nombre,
                    Fecha = @Fecha,
                    idLocal = @idLocal
                WHERE idEvento = @idEvento";

            int rows = db.Execute(sql, new
            {
                dto.Nombre,
                dto.Fecha,
                dto.idLocal,
                idEvento
            });

            return rows > 0;
        }

public void Publicar(int idEvento)
{
    using var db = Connection;

    var estado = db.ExecuteScalar<int?>(
        "SELECT Activo FROM Evento WHERE idEvento = @id",
        new { id = idEvento }
    );

    if (estado == -1)
        throw new Exception("No se puede publicar un evento cancelado.");

    db.Execute("UPDATE Evento SET Activo = 1 WHERE idEvento=@id", new { id = idEvento });
}

       public bool Cancelar(int idEvento)
{
    using var db = Connection;

    var estado = db.ExecuteScalar<int?>(
        "SELECT Activo FROM Evento WHERE idEvento = @id",
        new { id = idEvento }
    );

    if (estado is null)
        return false; 

    if (estado == -1)
        return false; 

    int rows = db.Execute(
        "UPDATE Evento SET Activo = -1 WHERE idEvento = @id",
        new { id = idEvento }
    );

    return rows > 0;
}
    
    }
}
