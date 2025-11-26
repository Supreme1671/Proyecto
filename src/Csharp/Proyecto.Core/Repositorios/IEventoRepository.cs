using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;

namespace Proyecto.Core.Repositorios
{
    public interface IEventoRepository
    {
        IEnumerable<Evento> GetAll();
        Evento? GetById(int idEvento);
        int Add(Evento evento);
        bool Update(int idEvento, EventoUpdateDTO dto);
        void Publicar(int idEvento);
        bool Cancelar(int idEvento);
    }
}
