using System.Collections.Generic;
using System.Threading.Tasks;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioVehiculo
    {
        Task Crear(Vehiculo vehiculo);
        Task<IEnumerable<Vehiculo>> Obtener(int idUsuario);
        Task<Vehiculo?> ObtenerPorId(int id, int idUsuario);
        Task Actualizar(Vehiculo vehiculo);
        Task Borrar(int id);
    }
}
