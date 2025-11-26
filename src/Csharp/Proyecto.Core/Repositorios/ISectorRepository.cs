using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;

namespace Proyecto.Core.Repositorios
{
    public interface ISectorRepository
    {
        IEnumerable<Sector> GetByLocal(int idLocal);
        void Add(int idLocal, Sector sector);
        bool Update(int idSector, SectorUpdateDTO dto);
        bool Delete(int idSector);
        Sector? GetById(int idSector);
    }
}
