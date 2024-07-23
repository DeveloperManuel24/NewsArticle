namespace NewsArticle.Models
{
    public class Pais
    {
        public int Id { get; set; }
        public string NombrePais { get; set; } = string.Empty;
        public ICollection<Provincia> Provincias { get; set; } = new List<Provincia>();
    }

    public class Provincia
    {
        public int Id { get; set; }
        public string NombreProvincia { get; set; } = string.Empty;
        public int IdPais { get; set; }

        public Pais Pais { get; set; } = new Pais();
        public ICollection<Comarca> Comarcas { get; set; } = new List<Comarca>();
        public ICollection<Distrito> Distritos { get; set; } = new List<Distrito>();
    }

    public class Comarca
    {
        public int Id { get; set; }
        public string NombreComarca { get; set; } = string.Empty;
        public int IdProvincia { get; set; }

        public Provincia Provincia { get; set; } = new Provincia();
        public ICollection<Distrito> Distritos { get; set; } = new List<Distrito>();
    }

    public class Distrito
    {
        public int Id { get; set; }
        public string NombreDistrito { get; set; } = string.Empty;
        public int IdProvincia { get; set; }

        public Provincia Provincia { get; set; } = new Provincia();
        public ICollection<Corregimiento> Corregimientos { get; set; } = new List<Corregimiento>();
    }

    public class Corregimiento
    {
        public int Id { get; set; }
        public string NombreCorregimiento { get; set; } = string.Empty;
        public int IdDistrito { get; set; }

        public Distrito Distrito { get; set; } = new Distrito();
    }
}
