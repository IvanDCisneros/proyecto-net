using System.ComponentModel.DataAnnotations;

namespace Dominio.Reportes
{
    public class IdClienteYRangoFechas
    {
        public string? IdCliente { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
    }
}
