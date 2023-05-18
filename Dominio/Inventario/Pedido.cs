using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Inventario
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public string? FechaRegistro { get; set; }
        public int IdFactura { get; set; }
        public int NumeroFactura { get; set; }
        public Int64 Identificacion { get; set; }
        public string? NombreCliente { get; set; }
        public int IdEstadoPedido { get; set; }
        public string? NombreEstadoPedido { get; set; }
        public Int64 NumeroGuia { get; set; }
        public string? EmpresaEnvio { get; set; }
        public string? IdUsuario { get; set; }

    }
}
