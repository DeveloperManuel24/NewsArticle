using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioIncautacion : IRepositorioIncautacion
    {
        private readonly string connectionString;

        public RepositorioIncautacion(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(Incautacion incautacion)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO incautaciones (tipo_incautacion, idusuario)
                VALUES (@TipoIncautacion, @idUsuario)
                RETURNING id_incautacion;", incautacion);
            incautacion.Id = id;
        }

        public async Task<IEnumerable<Incautacion>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Incautacion>(@"
                SELECT 
                    i.id_incautacion AS Id,
                    i.tipo_incautacion AS TipoIncautacion
                FROM incautaciones i
                WHERE i.idusuario = @idUsuario", new { idUsuario });
        }

        public async Task<Incautacion?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Incautacion>(
                @"SELECT   
                    i.id_incautacion AS Id,
                    i.tipo_incautacion AS TipoIncautacion
                  FROM incautaciones i
                  WHERE i.id_incautacion = @Id AND i.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(Incautacion incautacion)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE incautaciones
                  SET tipo_incautacion = @TipoIncautacion
                  WHERE id_incautacion = @Id AND idusuario = @idUsuario;", incautacion);
        }

        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM incautaciones WHERE id_incautacion = @Id", new { Id = id });
        }
    }
}
