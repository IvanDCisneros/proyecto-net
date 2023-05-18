using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Parametrizacion
{
    [Table("Municipios", Schema = "Parametrizacion")]
    public class Municipios
    {
        [Key]
        public int IdMunicipio { get; set; }
        public int IdDepartamento { get; set; }
        public string? CodigoDepartamento { get; set; }
        public string? CodigoMunicipio { get; set; }
        public string? NombreMunicipio { get; set; }
    }
}
