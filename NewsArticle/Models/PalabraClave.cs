using System.ComponentModel.DataAnnotations;

namespace NewsArticle.Models
{
    public class PalabraClave
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 30, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [MinLength(4, ErrorMessage = "El nombre debe tener al menos {1} caracteres")]
        [Display(Name = "nombre de palabra clave")]
        public required string NombrePalabraClave { get; set; }

        public int idUsuario { get; set; }
    }
}
