using System.ComponentModel.DataAnnotations;

namespace NewsArticle.Models
{
    public class Transporte
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres")]
        [Display(Name = "Tipo de Transporte")]
        public string TipoTransporte { get; set; } = null!;

        public int idUsuario { get; set; }
    }
}
