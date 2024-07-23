using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioEconomiaIlicita
    {
        Task Actualizar(EconomiaIlicita economiaIlicita);
        Task Borrar(int id);
        Task Crear(EconomiaIlicita economiaIlicita);
        Task<IEnumerable<EconomiaIlicita>> Obtener(int idUsuario);
        Task<EconomiaIlicita?> ObtenerPorId(int id, int idUsuario);
    }
}
