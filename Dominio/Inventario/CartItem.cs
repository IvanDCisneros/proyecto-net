using System.ComponentModel.DataAnnotations;

namespace Dominio.Inventario
{
    public class CartItem
    {
        [Key]
        public int IdProductoMercancia { get; set; }
        [Required]
        public string? NombreProducto { get; set; }
        [Required]
        public string? NombreGenero { get; set; }
        [Required]
        public string? NombreTalla { get; set; }
        [Required]
        public string? NombreColor { get; set; }
        [Required]
        public int ExistenciasBodega { get; set; }
        [Required]
        public decimal ValorVenta { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal SubTotal { get; set; }


    }
}
