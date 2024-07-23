using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioDroga : IRepositorioDroga
    {
        private readonly string connectionString;

        public RepositorioDroga(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(Droga droga)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO drogas (tipo_droga, idusuario)
                VALUES (@TipoDroga, @idUsuario)
                RETURNING id_drogas;", droga);
            droga.Id = id;
        }

        public async Task<IEnumerable<Droga>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Droga>(@"
                SELECT 
                    d.id_drogas AS Id,
                    d.tipo_droga AS TipoDroga
                FROM drogas d
                WHERE d.idusuario = @idUsuario", new { idUsuario });
        }

        public async Task<Droga?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Droga>(
                @"SELECT   
                    d.id_drogas AS Id,
                    d.tipo_droga AS TipoDroga
                  FROM drogas d
                  WHERE d.id_drogas = @Id AND d.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(Droga droga)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE drogas
                  SET tipo_droga = @TipoDroga
                  WHERE id_drogas = @Id AND idusuario = @idUsuario;", droga);
        }

        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM drogas WHERE id_drogas = @Id", new { Id = id });
        }
    }
}
