using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Parametrizacion
{
    [Table("Departamentos", Schema = "Parametrizacion")]
    public class Departamentos
    {
        [Key]
        public int IdDepartamento { get; set; }
        public string? CodigoDepartamento { get; set; }
        public string? NombreDepartamento { get; set; }
    }
}
