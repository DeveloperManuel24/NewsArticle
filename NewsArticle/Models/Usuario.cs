namespace NewsArticle.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string EmailNormalizado { get; set; } // Agregar este atributo
        public string PasswordHash { get; set; }
    }
}
