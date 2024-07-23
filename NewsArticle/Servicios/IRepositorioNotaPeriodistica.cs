using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioNotaPeriodistica
    {
        Task Crear(NotaPeriodistica notaPeriodistica);
        Task<IEnumerable<NotaPeriodistica>> Obtener(int idUsuario);
        Task<NotaPeriodistica?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(NotaPeriodistica notaPeriodistica);
        Task Borrar(int id);
        Task<IEnumerable<NotaPeriodistica>> ObtenerTodasLasNotas();
        Task<IEnumerable<NotaPeriodistica>> ObtenerConFiltros(int idUsuario, string titulo, int? palabraClaveId, DateTime? fechaHecho, int? AnoHecho, int? paisId);
        Task<IEnumerable<NotaPeriodistica>> ObtenerParaReporte(int idUsuario, string titulo, int? palabraClaveId, DateTime? fechaHecho, int? AnoHecho, int? paisId);
        Task<IEnumerable<string>> MostrarEnMapa(int id);
    }
}
