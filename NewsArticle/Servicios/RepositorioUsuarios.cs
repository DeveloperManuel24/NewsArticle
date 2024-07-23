using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Data;
using Microsoft.Extensions.Logging;

namespace NewsArticle.Servicios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectionString;
        private readonly ILogger<RepositorioUsuarios> logger;

        public RepositorioUsuarios(IConfiguration configuration, ILogger<RepositorioUsuarios> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
            this.logger = logger;
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var usuarioId = await connection.QuerySingleAsync<int>(@"
                 INSERT INTO Usuarios (nombre, email, emailNormalizado, contraseña)
                VALUES (@Nombre, @Email, @EmailNormalizado, @PasswordHash)
                RETURNING id_usuario;", usuario);

            await connection.ExecuteAsync("creardatosusuarionuevo", new { usuarioid = usuarioId },
                 commandType: CommandType.StoredProcedure);

            return usuarioId;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(
                @"SELECT id_usuario as Id, nombre as Nombre, email as Email, emailNormalizado as EmailNormalizado, 
          contraseña as PasswordHash 
          FROM Usuarios 
          WHERE emailNormalizado = @EmailNormalizado",
                new { EmailNormalizado = emailNormalizado });
        }
    }
}
