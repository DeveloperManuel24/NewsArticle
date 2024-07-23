using System.ComponentModel.DataAnnotations;

namespace NewsArticle.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(255, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres")]
        [Display(Name = "Tipo de Vehículo")]
        public string TipoVehiculo { get; set; } = null!;

        public int idUsuario { get; set; }
    }
}
