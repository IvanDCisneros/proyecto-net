using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Parametrizacion
{
    [Table("Color", Schema = "Parametrizacion")]
    public class Color
    {
        [Key]
        public int IdColor { get; set; }
        public string? Nombre { get; set; }
    }
}
