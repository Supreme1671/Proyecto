using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios
{
    public interface IFuncionRepository
    {
        IEnumerable<Funcion> GetAll();
        Funcion? GetById(int idFuncion);
        void Add(Funcion funcion);
        bool Update(int idFuncion, FuncionUpdateDTO dto);
        bool Cancelar(int idFuncion);
    }
}
