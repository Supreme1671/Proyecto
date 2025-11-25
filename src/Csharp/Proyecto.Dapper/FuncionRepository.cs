using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;

namespace Proyecto.Core.Repositorios.ReposDapper
{
    public class FuncionRepository : IFuncionRepository
    {
        private readonly string _connectionString;

        public FuncionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public IEnumerable<Funcion> GetAll()
        {
            using var db = Connection;
            return db.Query<Funcion>("SELECT * FROM Funcion");
        }

        public Funcion? GetById(int idFuncion)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Funcion>(
                "SELECT * FROM Funcion WHERE idFuncion = @idFuncion", new { idFuncion });
        }

        public void Add(Funcion funcion)
        {
            using var db = Connection;
            var sql = @"
            INSERT INTO Funcion (Descripcion, FechaHora, IdEvento, IdLocal)
            VALUES (@Descripcion, @FechaHora, @IdEvento, @IdLocal);
            SELECT LAST_INSERT_ID();";
            funcion.idFuncion = db.ExecuteScalar<int>(sql, funcion);
        }

        public void Update(Funcion funcion)
        {
            using var db = Connection;
            var sql = @"
            UPDATE Funcion
            SET Descripcion=@Descripcion, FechaHora=@FechaHora, IdEvento=@IdEvento, IdLocal=@IdLocal
            WHERE IdFuncion=@IdFuncion";
            db.Execute(sql, funcion);
        }


        public void Delete(int idFuncion)
        {
            using var db = Connection;
            db.Execute("DELETE FROM Funcion WHERE idFuncion=@idFuncion", new { idFuncion });
        }

        public bool Update(int idFuncion, FuncionUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
