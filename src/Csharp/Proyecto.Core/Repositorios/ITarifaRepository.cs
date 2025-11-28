using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;
namespace Proyecto.Core.Repositorios
{
    public interface ITarifaRepository
    {
        void Add(Tarifa tarifa);
        IEnumerable<Tarifa> GetByFuncionId(int idFuncion);
        Tarifa? GetById(int idTarifa);
        bool Update(int idTarifa, TarifaUpdateDTO dto);
        bool RestarStock(int idTarifa, int cantidad);
    }
}
