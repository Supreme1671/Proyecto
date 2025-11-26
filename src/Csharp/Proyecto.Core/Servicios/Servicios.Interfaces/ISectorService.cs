using Proyecto.Core.DTOs;

namespace Servicios.Interfaces
{
    public interface ISectorService
    {
        Task<SectorDTO?> GetByIdAsync(int id);
        Task<IEnumerable<SectorDTO>> GetAllAsync();
        Task<SectorDTO> CreateAsync(SectorCreateDTO dto);
        Task<SectorDTO> UpdateAsync(SectorUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
