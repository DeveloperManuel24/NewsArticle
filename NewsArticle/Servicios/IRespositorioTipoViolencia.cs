using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioTipoViolencia
    {
        Task Crear(TipoViolencia tipoViolencia);
        Task<IEnumerable<TipoViolencia>> Obtener(int idUsuario);
        Task<TipoViolencia?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(TipoViolencia tipoViolencia);
        Task Borrar(int id);
    }
}
