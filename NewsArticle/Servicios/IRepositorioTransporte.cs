using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioTransporte
    {
        Task Crear(Transporte transporte);
        Task<IEnumerable<Transporte>> Obtener(int idUsuario);
        Task<Transporte?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(Transporte transporte);
        Task Borrar(int id);
    }
}
