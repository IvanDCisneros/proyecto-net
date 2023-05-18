using Dominio.Clientes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Contabilidades
{
    public class Factura
    {
        [Key]
        public int IdFactura { get; set; }
        [Required]
        public int NumeroFactura { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public Int64 IdentificacionCliente { get; set; }
        [Required]
        public string? NombreCliente { get; set; }
        [Required]
        public string? CorreoElectronico { get; set; }
        [Required]
        public Int64 Telefono { get; set; }
        [Required]
        public string? Direccion { get; set; }
        [Required]
        public string? Ciudad { get; set; }
        [Required]
        public string? Departamento { get; set; }
        [Required]
        public int IdEstadoFactura { get; set; }
        [Required]
        public string? NombreEstadoFactura { get; set; }
        [Required]
        public decimal ValorTotal { get; set; }
        [Required]
        public DateTime Fecha { get; set; }

        public List<ItemFactura>? ListItemsFacturas { get; set; }
    }
}
