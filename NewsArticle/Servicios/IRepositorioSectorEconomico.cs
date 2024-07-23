using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioSectorEconomico
    {
        Task Crear(SectorEconomico sectorEconomico);
        Task<IEnumerable<SectorEconomico>> Obtener(int idUsuario);
        Task<SectorEconomico?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(SectorEconomico sectorEconomico);
        Task Borrar(int id);
    }
}
