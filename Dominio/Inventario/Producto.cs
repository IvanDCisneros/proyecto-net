using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Inventario
{
    [Table("Producto", Schema = "Inventario")]
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Descripcion { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        [Required]
        public string? RutaImagen { get; set; }
        [Required]
        public bool EstaActivo { get; set; }
    }
}
