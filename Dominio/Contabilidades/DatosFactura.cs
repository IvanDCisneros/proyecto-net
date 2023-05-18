using System.ComponentModel.DataAnnotations;

namespace Dominio.Contabilidades
{
    public class DatosFactura
    {
        public string? IdCliente { get; set; }
        public int IdProductoMercancia { get; set; }
        public decimal ValorVenta { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}
