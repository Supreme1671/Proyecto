using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;
namespace Proyecto.Core.Repositorios
{
    public interface IEventoRepository
    {
        IEnumerable<Evento> GetAll();
        Evento? GetById(int IdEvento);
        bool Add(Evento evento);
        void Update(Evento evento);
        void Publicar(int idEvento);
        bool Cancelar(int idEvento);
        bool Update(int idEvento, EventoUpdateDTO dto);
    }
}
