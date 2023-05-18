using System.ComponentModel.DataAnnotations;

namespace Dominio.Inventario
{
    public class ProductoMercancia
    {
        [Key]
        public int IdProductoMercancia { get; set; }
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public string? Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string? RutaImagen { get; set; }
        public int IdGenero { get; set; }
        public string? NombreGenero { get; set; }
        public int IdColor { get; set; }
        public string? NombreColor { get; set; }
        public int IdTalla { get; set; }
        public string? NombreTalla { get; set; }
        public int Existencia { get; set; }
        public decimal ValorCosto { get; set; }
        public decimal ValorVenta { get; set; }
        public string? IdUsuario { get; set; }

    }
}
