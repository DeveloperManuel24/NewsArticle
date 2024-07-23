using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioEconomiaIlicita : IRepositorioEconomiaIlicita
    {
        private readonly string connectionString;

        public RepositorioEconomiaIlicita(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(EconomiaIlicita economiaIlicita)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO economiailicita (nombre_economia,idusuario)
                VALUES (@NombreEconomiaIlicita,@idUsuario)
                RETURNING id_economia_ilicita;", economiaIlicita);
            economiaIlicita.Id = id;
        }
        public async Task<IEnumerable<EconomiaIlicita>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<EconomiaIlicita>(@"
                SELECT 
                    e.id_economia_ilicita AS Id,
                    e.nombre_economia AS NombreEconomiaIlicita
                FROM economiailicita e
                WHERE e.idusuario = @idUsuario", new { idUsuario });
        }


        public async Task<EconomiaIlicita?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<EconomiaIlicita>(
                @"SELECT   
            e.id_economia_ilicita AS Id,
            e.nombre_economia AS NombreEconomiaIlicita
          FROM economiailicita e
          WHERE e.id_economia_ilicita = @Id AND e.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }



        public async Task Actualizar(EconomiaIlicita economiaIlicita)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE economiailicita
          SET nombre_economia = @NombreEconomiaIlicita
          WHERE id_economia_ilicita = @Id AND idusuario = @idUsuario;", economiaIlicita);
        }


        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM economiailicita WHERE id_economia_ilicita = @Id", new { Id = id });
        }
    }
}
