using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioTransporte : IRepositorioTransporte
    {
        private readonly string connectionString;

        public RepositorioTransporte(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(Transporte transporte)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO transporte (tipo_transporte, idusuario)
                VALUES (@TipoTransporte, @idUsuario)
                RETURNING id_transporte;", transporte);
            transporte.Id = id;
        }

        public async Task<IEnumerable<Transporte>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Transporte>(@"
                SELECT 
                    t.id_transporte AS Id,
                    t.tipo_transporte AS TipoTransporte
                FROM transporte t
                WHERE t.idusuario = @idUsuario", new { idUsuario });
        }

        public async Task<Transporte?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transporte>(
                @"SELECT   
                    t.id_transporte AS Id,
                    t.tipo_transporte AS TipoTransporte
                  FROM transporte t
                  WHERE t.id_transporte = @Id AND t.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(Transporte transporte)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE transporte
                  SET tipo_transporte = @TipoTransporte
                  WHERE id_transporte = @Id AND idusuario = @idUsuario;", transporte);
        }

        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM transporte WHERE id_transporte = @Id", new { Id = id });
        }
    }
}
