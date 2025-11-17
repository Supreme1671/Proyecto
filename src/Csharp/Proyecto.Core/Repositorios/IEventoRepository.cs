using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;
namespace Proyecto.Core.Repositorios
{
    public interface IEventoRepository
    {
        IEnumerable<Evento> GetAll();
        Evento? GetById(int idEvento);
        void Add(Evento evento);
        void Update(Evento evento);
        void Publicar(int idEvento);
        void Cancelar(int idEvento);
        void Delete(int idEvento);
        bool Update(int idEvento, EventoUpdateDTO dto);
    }
}
