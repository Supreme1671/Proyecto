using Proyecto.Core.Entidades;
using Proyecto.Core.DTOs;
namespace Proyecto.Core.Repositorios;

public interface ISectorRepository
{
    IEnumerable<Sector> GetAll();
    Sector? GetById(int idSector);
    void Add(Sector sector);
    void Update(Sector sector);
    bool Delete(int idSector);
    void Add(int idLocal, Sector sector);
    bool Update(int idSector, SectorDTO dto);
}