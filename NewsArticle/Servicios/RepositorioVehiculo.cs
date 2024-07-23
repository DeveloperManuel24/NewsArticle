using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioVehiculo : IRepositorioVehiculo
    {
        private readonly string connectionString;

        public RepositorioVehiculo(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(Vehiculo vehiculo)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO vehiculos (tipo_vehiculo, idusuario)
                VALUES (@TipoVehiculo, @idUsuario)
                RETURNING id_vehiculo;", vehiculo);
            vehiculo.Id = id;
        }

        public async Task<IEnumerable<Vehiculo>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Vehiculo>(@"
                SELECT 
                    v.id_vehiculo AS Id,
                    v.tipo_vehiculo AS TipoVehiculo
                FROM vehiculos v
                WHERE v.idusuario = @idUsuario", new { idUsuario });
        }

        public async Task<Vehiculo?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Vehiculo>(
                @"SELECT   
                    v.id_vehiculo AS Id,
                    v.tipo_vehiculo AS TipoVehiculo
                  FROM vehiculos v
                  WHERE v.id_vehiculo = @Id AND v.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(Vehiculo vehiculo)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE vehiculos
                  SET tipo_vehiculo = @TipoVehiculo
                  WHERE id_vehiculo = @Id AND idusuario = @idUsuario;", vehiculo);
        }

        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM vehiculos WHERE id_vehiculo = @Id", new { Id = id });
        }
    }
}
