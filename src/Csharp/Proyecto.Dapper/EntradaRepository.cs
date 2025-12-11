using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios.ReposDapper
{
    public class EntradaRepository : IEntradaRepository
    {
        private readonly string _connectionString;

        public EntradaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public IEnumerable<Entrada> GetAll()
        {
            using var db = Connection;
            string sql = "SELECT * FROM Entrada;";
            return db.Query<Entrada>(sql);
        }

        public Entrada? GetById(int idEntrada)
        {
            using var db = Connection;
            string sql = "SELECT * FROM Entrada WHERE IdEntrada = @IdEntrada;";
            return db.QueryFirstOrDefault<Entrada>(sql, new { idEntrada });
        }

        public void Update(Entrada entrada)
        {
            using var db = Connection;
            string sql = @"
            UPDATE Entrada SET
                Precio = @Precio,
                idFuncion = @idFuncion,
                idCliente = @idCliente,
                Usada = @Usada,
                Anulada = @Anulada,
                Numero = @Numero,
                QR = @QR
            WHERE IdEntrada = @IdEntrada;";
            db.Execute(sql, entrada);
        }

        public void Anular(int idEntrada)
        {
            using var db = Connection;
            string sqlCheck = "SELECT Estado FROM Entrada WHERE IdEntrada = @IdEntrada;";
            var estado = db.QueryFirstOrDefault<string>(sqlCheck, new { idEntrada });

            if (estado == null) throw new Exception("La entrada no existe.");
            if (estado == "Anulada") throw new Exception("La entrada ya está anulada.");

            string sqlUpdate = "UPDATE Entrada SET Estado = 'Anulada' WHERE IdEntrada = @IdEntrada;";
            db.Execute(sqlUpdate, new { idEntrada });
        }
       public int Add(Entrada entrada)
{
    using var connection = new MySqlConnection(_connectionString);
    connection.Open();

    var sql = @"
        INSERT INTO Entrada
            (Precio, QR, Anulada, Usada, Numero, IdDetalleOrden, idSector, idFuncion, idTarifa, idCliente, Estado)
        VALUES
            (@Precio, @QR, @Anulada, @Usada, @Numero, @IdDetalleOrden, @IdSector, @IdFuncion, @IdTarifa, @IdCliente, @Estado);
        SELECT LAST_INSERT_ID();
    ";

    var id = connection.ExecuteScalar<int>(sql, new
    {
        Precio = entrada.Precio,
        QR = entrada.QR,
        Anulada = entrada.Anulada,
        Usada = entrada.Usada,
        Numero = entrada.Numero,
        IdDetalleOrden = entrada.IdDetalleOrden,
        IdSector = entrada.IdSector,
        IdFuncion = entrada.IdFuncion,
        IdTarifa = entrada.IdTarifa,
        IdCliente = entrada.IdCliente,
        Estado = string.IsNullOrWhiteSpace(entrada.Estado) ? "Disponible" : entrada.Estado
    });

    entrada.IdEntrada = id;

    return id;
}


        void IEntradaRepository.Add(Entrada entrada)
{
    
    var id = this.Add(entrada);
    entrada.IdEntrada = id;
}

    }
}