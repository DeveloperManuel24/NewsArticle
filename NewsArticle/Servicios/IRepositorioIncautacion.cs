using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioIncautacion
    {
        Task Crear(Incautacion incautacion);
        Task<IEnumerable<Incautacion>> Obtener(int idUsuario);
        Task<Incautacion?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(Incautacion incautacion);
        Task Borrar(int id);
    }
}
