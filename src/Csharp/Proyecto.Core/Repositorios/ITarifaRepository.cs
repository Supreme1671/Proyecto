using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;
namespace Proyecto.Core.Repositorios
{
    public interface ITarifaRepository
    {
        void Add(Tarifa tarifa);
        IEnumerable<Tarifa> GetByFuncionId(int IdFuncion);
        Tarifa? GetById(int idTarifa);
        void Update(Tarifa tarifa);
        bool Update(int idTarifa, TarifaUpdateDTO dto);
    }
}
