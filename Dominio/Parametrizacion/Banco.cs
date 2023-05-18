using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Parametrizacion
{
    [Table("Banco", Schema = "Parametrizacion")]
    public class Banco
    {
        [Key]
        public int IdBanco { get; set; }
        public string? Nombre { get; set; }
    }
}
