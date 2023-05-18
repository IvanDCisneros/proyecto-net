using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Reportes
{
    public class ReporteFactura
    {
        [Key]
        public int IdFactura { get; set; }
        public int NumeroFactura { get; set; }
        public Int64 Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? Fecha { get; set; }
        public string? NombreEstado { get; set; }
        public string? Descripcion { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
