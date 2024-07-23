using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using NewsArticle.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioPalabrasClave : IRepositorioPalabrasClave
    {
        private readonly string connectionString;

        public RepositorioPalabrasClave(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(PalabraClave palabraClave)
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO palabraclave (palabra_clave, idUsuario)
                VALUES (@NombrePalabraClave, @idUsuario)
                RETURNING id_palabra_clave;", palabraClave);
                palabraClave.Id = id;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ocurrió un error al intentar insertar una palabra clave en la base de datos.", ex);
            }
        }

        public async Task<IEnumerable<PalabraClave>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<PalabraClave>(
                @"SELECT id_palabra_clave AS Id, palabra_clave AS NombrePalabraClave, idUsuario
                  FROM palabraclave
                  WHERE idUsuario = @idUsuario", new { idUsuario });
        }

        public async Task<PalabraClave?> ObtenerPorId(int id, int idUsuario) // Trae la información que el usuario quiere editar o ver
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<PalabraClave>(
                @"SELECT  id_palabra_clave,palabra_clave AS NombrePalabraClave
                  FROM palabraclave
                  WHERE id_palabra_clave = @Id AND idUsuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(PalabraClave palabraClave)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                        @"UPDATE palabraclave
	                    SET palabra_clave = @NombrePalabraClave
	                    WHERE id_palabra_clave = @Id;", palabraClave);
        }
        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM palabraclave WHERE id_palabra_clave = @Id", new { Id = id });
        }


    }
}
