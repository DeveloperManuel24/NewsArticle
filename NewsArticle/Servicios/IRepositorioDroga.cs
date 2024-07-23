using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioDroga
    {
        Task Crear(Droga droga);
        Task<IEnumerable<Droga>> Obtener(int idUsuario);
        Task<Droga?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(Droga droga);
        Task Borrar(int id);
    }
}
