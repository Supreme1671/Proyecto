using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios;

public interface IEntradaRepository
{
    IEnumerable<Entrada> GetAll();
    Entrada? GetById(int IdEntrada);
    void Anular(int IdEntrada);
    void Update(Entrada entrada);

    void Add(Entrada entrada);
}