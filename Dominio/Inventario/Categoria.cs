using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Inventario
{
    [Table("Categoria", Schema = "Inventario")]
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? RutaImagen { get; set; }
        [Required]
        public bool EstaActivo { get; set; }
    }
}
