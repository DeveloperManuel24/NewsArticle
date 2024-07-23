using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using NewsArticle.Models;

namespace NewsArticle.Servicios
{
    public class RepositorioPais : IRepositorioPais
    {
        private readonly string connectionString;

        public RepositorioPais(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }


        public async Task<IEnumerable<Pais>> Obtener()
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Pais>(
                @"SELECT id_pais AS Id, nombre_pais AS NombrePais
                  FROM pais");
        }

        public async Task<IEnumerable<Provincia>> ObtenerProvincias(int paisId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Provincia>(
                @"SELECT id_provincia AS Id, nombre_provincia AS NombreProvincia
                  FROM provincia_departamento
                  WHERE id_pais = @PaisId", new { PaisId = paisId });
        }

        public async Task<IEnumerable<Comarca>> ObtenerComarcas(int provinciaId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Comarca>(
                @"SELECT id_comarca AS Id, nombre_comarca AS NombreComarca
                  FROM comarca
                  WHERE id_provincia = @ProvinciaId", new { ProvinciaId = provinciaId });
        }

        public async Task<IEnumerable<Distrito>> ObtenerDistritos(int provinciaId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Distrito>(
                @"SELECT id_distrito AS Id, nombre_distrito AS NombreDistrito
                  FROM distrito_municipio
                  WHERE id_provincia = @ProvinciaId", new { ProvinciaId = provinciaId });
        }

        public async Task<IEnumerable<Corregimiento>> ObtenerCorregimientos(int distritoId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<Corregimiento>(
                @"SELECT id_corregimiento AS Id, nombre_corregimiento AS NombreCorregimiento
                  FROM corregimiento_aldea
                  WHERE id_distrito = @DistritoId", new { DistritoId = distritoId });
        }
    }
}
