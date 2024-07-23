using System.ComponentModel.DataAnnotations;

namespace NewsArticle.Models
{
    public class MatasArbustos
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(255, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres")]
        [Display(Name = "Tipo de Matas o Arbustos")]
        public string TipoMatasArbustos { get; set; } = null!;

        public int idUsuario { get; set; }
    }
}
