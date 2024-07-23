using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioPais
    {
        Task<IEnumerable<Pais>> Obtener();
        Task<IEnumerable<Comarca>> ObtenerComarcas(int provinciaId);
        Task<IEnumerable<Corregimiento>> ObtenerCorregimientos(int distritoId);
        Task<IEnumerable<Distrito>> ObtenerDistritos(int comarcaId);
        Task<IEnumerable<Provincia>> ObtenerProvincias(int paisId);
    }
}
