/*
using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Servicios.Interfaces;
using Servicios.Interfaces;

namespace Servicios.Servicios
{
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _sectorRepository;

        public SectorService(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }

        public async Task<IEnumerable<SectorDTO>> GetAllAsync()
        {
            var sectores = await _sectorRepository.GetAllAsync();

            return sectores.Select(s => new SectorDTO
            {
                idSector = s.idSector,
                Nombre = s.Nombre,
                idLocal = s.idLocal,
                Capacidad = s.Capacidad,
                Precio = s.Precio
            });
        }

        public async Task<SectorDTO?> GetByIdAsync(int id)
        {
            var sector = await _sectorRepository.GetByIdAsync(id);

            if (sector == null)
                return null;

            return new SectorDTO
            {
                idSector = sector.idSector,
                Nombre = sector.Nombre,
                idLocal = sector.idLocal,
                Capacidad = sector.Capacidad,
                Precio = sector.Precio
            };
        }

        public async Task<SectorDTO> CreateAsync(SectorCreateDTO dto)
        {
            var sector = new Sector
            {
                Nombre = dto.Nombre,
                idLocal = dto.idLocal,
                Capacidad = dto.Capacidad,
                Precio = dto.Precio
            };

            await _sectorRepository.CreateAsync(sector);

            return new SectorDTO
            {
                idSector = sector.idSector,
                Nombre = sector.Nombre,
                idLocal = sector.idLocal,
                Capacidad = sector.Capacidad,
                Precio = sector.Precio
            };
        }

        public async Task<SectorDTO?> UpdateAsync(int idSector, SectorUpdateDTO dto)
        {
            // Obtener sector viejo
            var existente = await _sectorRepository.GetByIdAsync(idSector);
            if (existente == null)
                return null;

            // Aplicar cambios
            existente.Nombre = dto.Nombre;
            existente.Capacidad = dto.Capacidad;
            existente.Precio = dto.Precio;

            await _sectorRepository.UpdateAsync(existente);

            // Devolver actualizado
            return new SectorDTO
            {
                idSector = existente.idSector,
                Nombre = existente.Nombre,
                idLocal = existente.idLocal,
                Capacidad = existente.Capacidad,
                Precio = existente.Precio
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _sectorRepository.DeleteAsync(id);
        }

        public Task<SectorDTO> UpdateAsync(SectorUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
*/