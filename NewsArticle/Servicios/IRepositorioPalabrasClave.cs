using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public interface IRepositorioPalabrasClave
    {
        Task Actualizar(PalabraClave palabraClave);
        Task Borrar(int id);
        Task Crear(PalabraClave palabraClave);
        Task<IEnumerable<PalabraClave>> Obtener(int usuarioId);
        Task<PalabraClave?> ObtenerPorId(int id, int idUsuario);
    }
}
