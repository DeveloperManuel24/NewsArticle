using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioMatasArbustos
    {
        Task Crear(MatasArbustos matasArbustos);
        Task<IEnumerable<MatasArbustos>> Obtener(int idUsuario);
        Task<MatasArbustos?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(MatasArbustos matasArbustos);
        Task Borrar(int id);
    }
}
