using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsArticle.Models
{
    public class EconomiaIlicita
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres")]
        [Display(Name = "Nombre de Economía Ilícita")]
        public string NombreEconomiaIlicita { get; set; } = null!;

        public int idUsuario { get; set; }

    }
}
