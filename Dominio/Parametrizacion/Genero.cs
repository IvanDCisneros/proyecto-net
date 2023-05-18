using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Parametrizacion
{
    [Table("Genero", Schema = "Parametrizacion")]
    public class Genero
    {
        [Key]
        public int IdGenero { get; set; }
        public string? Nombre { get; set; }
    }
}
