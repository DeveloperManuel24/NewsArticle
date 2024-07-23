using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioTipoViolencia : IRepositorioTipoViolencia
    {
        private readonly string connectionString;

        public RepositorioTipoViolencia(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(TipoViolencia tipoViolencia)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO tipoviolencia (tipo_violencia, idusuario)
                VALUES (@TipoViolenciaNombre, @idUsuario)
                RETURNING id_tipoviolencia;", tipoViolencia);
            tipoViolencia.Id = id;
        }

        public async Task<IEnumerable<TipoViolencia>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<TipoViolencia>(@"
                SELECT 
                    t.id_tipoviolencia AS Id,
                    t.tipo_violencia AS TipoViolenciaNombre
                FROM tipoviolencia t
                WHERE t.idusuario = @idUsuario", new { idUsuario });
        }

        public async Task<TipoViolencia?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoViolencia>(
                @"SELECT   
                    t.id_tipoviolencia AS Id,
                    t.tipo_violencia AS TipoViolenciaNombre
                  FROM tipoviolencia t
                  WHERE t.id_tipoviolencia = @Id AND t.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(TipoViolencia tipoViolencia)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE tipoviolencia
                  SET tipo_violencia = @TipoViolenciaNombre
                  WHERE id_tipoviolencia = @Id AND idusuario = @idUsuario;", tipoViolencia);
        }

        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM tipoviolencia WHERE id_tipoviolencia = @Id", new { Id = id });
        }
    }
}
