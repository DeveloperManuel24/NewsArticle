using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioMatasArbustos : IRepositorioMatasArbustos
    {
        private readonly string connectionString;

        public RepositorioMatasArbustos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(MatasArbustos matasArbustos)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO matas_arbustos (tipo_matas_arbustos, idusuario)
                VALUES (@TipoMatasArbustos, @idUsuario)
                RETURNING id_matas_arbustos;", matasArbustos);
            matasArbustos.Id = id;
        }

        public async Task<IEnumerable<MatasArbustos>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<MatasArbustos>(@"
                SELECT 
                    m.id_matas_arbustos AS Id,
                    m.tipo_matas_arbustos AS TipoMatasArbustos
                FROM matas_arbustos m
                WHERE m.idusuario = @idUsuario", new { idUsuario });
        }

        public async Task<MatasArbustos?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<MatasArbustos>(
                @"SELECT   
                    m.id_matas_arbustos AS Id,
                    m.tipo_matas_arbustos AS TipoMatasArbustos
                  FROM matas_arbustos m
                  WHERE m.id_matas_arbustos = @Id AND m.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(MatasArbustos matasArbustos)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE matas_arbustos
                  SET tipo_matas_arbustos = @TipoMatasArbustos
                  WHERE id_matas_arbustos = @Id AND idusuario = @idUsuario;", matasArbustos);
        }

        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM matas_arbustos WHERE id_matas_arbustos = @Id", new { Id = id });
        }
    }
}
