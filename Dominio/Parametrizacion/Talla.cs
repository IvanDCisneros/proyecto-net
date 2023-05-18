using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Parametrizacion
{
    [Table("Talla", Schema = "Parametrizacion")]
    public class Talla
    {
        [Key]
        public int IdTalla { get; set; }
        public string? Nombre { get; set; }
    }
}
