using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioSectorEconomico : IRepositorioSectorEconomico
    {
        private readonly string connectionString;

        public RepositorioSectorEconomico(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task Crear(SectorEconomico sectorEconomico)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO sectoreconomico (nombre_sector, idusuario)
                VALUES (@NombreSector, @idUsuario)
                RETURNING id_sector_economico;", sectorEconomico);
            sectorEconomico.Id = id;
        }

        public async Task<IEnumerable<SectorEconomico>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<SectorEconomico>(@"
                SELECT 
                    s.id_sector_economico AS Id,
                    s.nombre_sector AS NombreSector
                FROM sectoreconomico s
                WHERE s.idusuario = @idUsuario", new { idUsuario });
        }

        public async Task<SectorEconomico?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<SectorEconomico>(
                @"SELECT   
                    s.id_sector_economico AS Id,
                    s.nombre_sector AS NombreSector
                  FROM sectoreconomico s
                  WHERE s.id_sector_economico = @Id AND s.idusuario = @idUsuario;", new { Id = id, idUsuario });
        }

        public async Task Actualizar(SectorEconomico sectorEconomico)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE sectoreconomico
                  SET nombre_sector = @NombreSector
                  WHERE id_sector_economico = @Id AND idusuario = @idUsuario;", sectorEconomico);
        }

        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM sectoreconomico WHERE id_sector_economico = @Id", new { Id = id });
        }
    }
}
