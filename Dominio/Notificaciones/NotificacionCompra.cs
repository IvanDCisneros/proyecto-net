using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Notificaciones
{
    public class NotificacionCompra
    {
        [Key]
        public Int64 Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? CorreoElectronico { get; set; }
        public DateTime Fecha { get; set; }
        public int NumeroFactura { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Direccion { get; set; }
        public string? Municipio { get; set; }
        public string? Departamento { get; set; }
    }
}
