using System.ComponentModel.DataAnnotations;

namespace Dominio.Contabilidades
{
    public class ItemFactura
    {
        [Key]
        public int IdItemFactura { get; set; }
        [Required]
        public int IdFactura { get; set; }
        [Required]
        public int IdProductoMercancia { get; set; }
        [Required]
        public string? CodigoArticulo { get; set; }
        [Required]
        public string? Descripcion { get; set; }
        [Required]
        public decimal ValorUnitario { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal SubTotal { get; set; }
    }
}
