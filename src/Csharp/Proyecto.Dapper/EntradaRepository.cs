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
            string sql = "SELECT * FROM Entrada WHERE idEntrada = @idEntrada;";
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
            WHERE idEntrada = @idEntrada;";
            db.Execute(sql, entrada);
        }

        public void Anular(int idEntrada)
        {
            using var db = Connection;
            string sqlCheck = "SELECT Estado FROM Entrada WHERE idEntrada = @idEntrada;";
            var estado = db.QueryFirstOrDefault<string>(sqlCheck, new { idEntrada });

            if (estado == null) throw new Exception("La entrada no existe.");
            if (estado == "Anulada") throw new Exception("La entrada ya está anulada.");

            string sqlUpdate = "UPDATE Entrada SET Estado = 'Anulada' WHERE idEntrada = @idEntrada;";
            db.Execute(sqlUpdate, new { idEntrada });
        }
        public int Add(Entrada entrada)
{
    using var connection = new MySqlConnection(_connectionString);
    connection.Open();

    var sql = @"
        INSERT INTO Entrada
            (Precio, IdTarifa, IdFuncion, Estado, Usada, Anulada, Numero, IdSector, IdDetalleOrden, idCliente, QR)
        VALUES
            (@Precio, @IdTarifa, @IdFuncion, @Estado, @Usada, @Anulada, @Numero, @IdSector, @IdDetalleOrden, @IdCliente, @Qr);
        SELECT LAST_INSERT_ID();";

    // Dapper mapeará int? -> NULL automáticamente si es null
    var id = connection.ExecuteScalar<int>(sql, new
    {
        Precio = entrada.Precio,
        IdTarifa = entrada.idTarifa,
        IdFuncion = entrada.idFuncion,
        Estado = string.IsNullOrWhiteSpace(entrada.Estado) ? "Disponible" : entrada.Estado,
        Usada = entrada.Usada,
        Anulada = entrada.Anulada,
        Numero = entrada.Numero,
        IdSector = entrada.idSector,
        IdDetalleOrden = entrada.IdDetalleOrden,
        IdCliente = entrada.idCliente,
        Qr = entrada.QR
    });

    return id;
}

        void IEntradaRepository.Add(Entrada entrada)
        {
            throw new NotImplementedException();
        }
    }
}