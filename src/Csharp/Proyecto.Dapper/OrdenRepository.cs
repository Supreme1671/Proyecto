using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios.ReposDapper;
public class OrdenRepository : IOrdenRepository
{
    private readonly string _connectionString;

    public OrdenRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    // Crear orden (INSERT)
    public void Add(Orden orden)
{
    using var db = Connection;

    string sql = @"
        INSERT INTO Orden (idCliente, Fecha, Estado)
        VALUES (@idCliente, @Fecha, @Estado);
        SELECT LAST_INSERT_ID();";

    var id = db.ExecuteScalar<int>(sql, orden);
    orden.idOrden = id;

    // Insertar detalles
    foreach (var detalle in orden.Detalles)
    {
        string sqlDetalle = @"
            INSERT INTO DetalleOrden (idOrden, idEvento, idTarifa, Cantidad, PrecioUnitario)
            VALUES (@idOrden, @idEvento, @idTarifa, @Cantidad, @PrecioUnitario);";

        detalle.IdOrden = id;

        db.Execute(sqlDetalle, new {
            idOrden = id,
            idEvento = detalle.IdEvento,
            idTarifa = detalle.IdTarifa,
            Cantidad = detalle.Cantidad,
            PrecioUnitario = detalle.PrecioUnitario
        });
    }
}

    // Listar todas las órdenes
    public IEnumerable<Orden> GetAll()
    {
        using var db = Connection;
        string sql = "SELECT * FROM Orden;";
        var ordenes = db.Query<Orden>(sql).ToList();

        foreach (var orden in ordenes)
        {
            string sqlDetalle = "SELECT * FROM DetalleOrden WHERE idOrden = @idOrden;";
            orden.Detalles = db.Query<DetalleOrden>(sqlDetalle, new { IdOrden = orden.idOrden }).ToList();
        }

        return ordenes;
    }

    // Eliminarpor ID
    public Orden? GetById(int idOrden)
    {
        using var db = Connection;
        string sql = "SELECT * FROM Orden WHERE idOrden = @idOrden;";
        var orden = db.QueryFirstOrDefault<Orden>(sql, new { IdOrden = idOrden });

        if (orden != null)
        {
            string sqlDetalle = "SELECT * FROM DetalleOrden WHERE idOrden = @idOrden;";
            orden.Detalles = db.Query<DetalleOrden>(sqlDetalle, new { IdOrden = idOrden }).ToList();
        }

        return orden;
    }

    // Actualizar orden (ej: cambiar estado o usuario)
    public void Update(Orden orden)
    {
        using var db = Connection;
        string sql = @"
                UPDATE Orden 
                SET idCliente = @idCliente,
                Estado = @Estado
                WHERE idOrden = @idOrden;";
        db.Execute(sql, orden);
    }
bool IOrdenRepository.Pagar(int idOrden)
{
    using var db = Connection;
    string sql = "UPDATE Orden SET Estado = 'Pagada' WHERE idOrden = @idOrden;";

    int filas = db.Execute(sql, new { idOrden });
    return filas > 0;
}


public bool Cancelar(int idOrden)
{
    using var db = Connection;

    string sql = @"
        UPDATE Orden 
        SET Estado = 'Cancelada' 
        WHERE idOrden = @idOrden AND Estado = 'Creada';
    ";

    int filas = db.Execute(sql, new { idOrden });

    return filas > 0;
}


}