using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsArticle.Models
{
    public class NotaPeriodistica
    {
        public int Id { get; set; }

        // Información de la noticia y el hecho
        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "La URL de la fuente es obligatoria.")]
        [Url(ErrorMessage = "La URL no es válida.")]
        public string UrlFuente { get; set; } = null!;

        [Required(ErrorMessage = "El nombre de la fuente es obligatorio.")]
        public string NombreFuente { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaPublicacion { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100.")]
        public int AnoPublicacion { get; set; }

        [Required(ErrorMessage = "La fecha del hecho es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaHecho { get; set; }

        [Required(ErrorMessage = "El año del hecho es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100.")]
        public int AnoHecho { get; set; }
        [Required(ErrorMessage = "La nota periodística es obligatoria.")]
        public string NotaGrande { get; set; }

        // Área geográfica
        [Required(ErrorMessage = "El país es obligatorio.")]
        public int IdPais { get; set; }

        public int? IdDepartamento { get; set; }
        public int? IdComarca { get; set; }
        public int? IdMunicipio { get; set; }
        public int? IdAldea { get; set; }

        [StringLength(100, ErrorMessage = "La ruta de carretera no puede superar los 100 caracteres.")]
        public string? rutaCarretera { get; set; }

        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        public double Latitud { get; set; }

        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        public double Longitud { get; set; }

        // Decomiso de droga por tipo de transporte
        public string? NombreCartel { get; set; }
        public string? NombreOperacionPolicial { get; set; }
        public int? huboViolencia { get; set; }
        public int? tipoTransporte { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad en kilos debe ser positiva.")]
        public int? CantidadEnKilos { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto en dólares debe ser positivo.")]
        public decimal? MontoEnDolaresTransporte { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de pistas destruidas debe ser positivo.")]
        public int? NumeroPistasDestruidas { get; set; }

        // Sobornos
        [Range(0, double.MaxValue, ErrorMessage = "El monto en dólares debe ser positivo.")]
        public decimal? MontoEnDolaresIntentoSoborno { get; set; }

        [StringLength(500, ErrorMessage = "La breve nota no puede superar los 500 caracteres.")]
        public string? BreveNota { get; set; }

        public string? InstitucionResponsable { get; set; }
        public string? InstitucionesDeApoyo { get; set; }

        // Sector económico afectado
        public int? IdSectorEconomico { get; set; }

        // Tipo de economía ilícita
        public int? IdEconomiaIlicita { get; set; }

        public string? NombreFauna { get; set; }
        public int? CantidadFauna { get; set; }
        public string? NombreFlora { get; set; }
        public int? CantidadFlora { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de personas en trata debe ser positiva.")]
        public int? CantidadPersonasTrata { get; set; }

        public int? NacionalidadTrata { get; set; }

        // Detenidos
        [Range(0, int.MaxValue, ErrorMessage = "El número de detenidos debe ser positivo.")]
        public int? NumeroDetenidos { get; set; }

        public string? NacionalidadDetenidos { get; set; }

        // Vinculado con deforestación
        public string? NombreAreaProtegida { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de hectáreas debe ser positivo.")]
        public int? NumeroHectareasAreaProtegida { get; set; }

        // Incautaciones
        public int? IdIncautacion { get; set; }
        public int? IdDroga { get; set; }
        public int? IdMatasArbustos { get; set; }
        public int? NumeroMatasArbustos { get; set; }
        public int? NumeroHectareas { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto en dólares debe ser positivo.")]
        public decimal? MontoEnDolaresMatasArbustos { get; set; }

        public int? IdVehiculo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto en dólares debe ser positivo.")]
        public decimal? MontoEnDolaresDinero { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto en dólares debe ser positivo.")]
        public decimal? MontoEnDolaresJoyas { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de inmuebles debe ser positivo.")]
        public int? NumeroInmuebles { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de fincas debe ser positivo.")]
        public int? NumeroFincas { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de hectáreas debe ser positivo.")]
        public int? NumeroHectareasTerrenos { get; set; }

        // Extra fields
        [Required(ErrorMessage = "La palabra clave es obligatoria.")]
        public int IdPalabraClave { get; set; }

        public int? IdUsuario { get; set; }


        //CAMPOS PARA MOSTRAR EN LOS REPORTES
        [NotMapped]
        public string? MostrarNombrePalabraClave { get; set; }

        [NotMapped]
        public string? MostrarNombrePaís { get; set; }

        [NotMapped]
        public string? MostrarNombreDepartamento { get; set; }

        [NotMapped]
        public string? MostrarNombreComarca { get; set; }

        [NotMapped]
        public string? MostrarNombreMunicipio { get; set; }

        [NotMapped]
        public string? MostrarNombreAldea { get; set; }

        [NotMapped]
        public string? MostrarNombreHuboViolencia { get; set; }

        [NotMapped]
        public string? MostrarNombreTipoTransporte { get; set; }

        [NotMapped]
        public string? MostrarNombreSectorEconómico { get; set; }

        [NotMapped]
        public string? MostrarNombreEconomíaIlícita { get; set; }

        [NotMapped]
        public string? MostrarNombreNacionalidadTrata { get; set; } // (recuerda que este es el mismo FK de pais)

        [NotMapped]
        public string? MostrarNombreIncautación { get; set; }

        [NotMapped]
        public string? MostrarNombreDroga { get; set; }

        [NotMapped]
        public string? MostrarNombreMatasArbustos { get; set; }

        [NotMapped]
        public string? MostrarNombreVehículo { get; set; }
        [NotMapped]
        public string? nombrePalabraClave { get; set; }
    }
}
