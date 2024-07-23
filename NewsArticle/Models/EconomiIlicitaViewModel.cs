using System.ComponentModel.DataAnnotations;

namespace NewsArticle.Models
{
    public class EconomiIlicitaViewModel
    {
       
        public required IEnumerable<EconomiaIlicita> economiaIlicita { get; set; }
    }
}
