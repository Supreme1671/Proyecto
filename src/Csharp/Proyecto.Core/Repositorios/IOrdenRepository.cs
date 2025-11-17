using Proyecto.Core.Entidades;
namespace Proyecto.Core.Repositorios;

public interface IOrdenRepository
{
    IEnumerable<Orden> GetAll();
    Orden? GetById(int idOrden);
    void Add(Orden orden);
    void Update(Orden orden);
    bool Pagar(int idOrden);

    void Cancelar(int IdOrden);
    //void Add(Orden nueva);
}

