using Dominio.Reportes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace MundoIndigoAPI.Controllers.Clientes
{
    [Route("api/Reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly AplicationDBContext _db;
        private readonly IConfiguration _config;

        public ReportesController(AplicationDBContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [Route("ReporteFacturas")]
        [HttpPost]
        public async Task<ActionResult<ReporteFactura>> ReporteFacturas(IdClienteYRangoFechas idClienteYRangoFechas)
        {
            try
            {
                idClienteYRangoFechas.IdCliente = Dominio.Utilidades.Seguridad.DesEncriptar(idClienteYRangoFechas.IdCliente);
                string sentencia = $"EXEC Contabilidades.ObtenerFacturasPorIdClienteYRangoFechas '{idClienteYRangoFechas.IdCliente}','{idClienteYRangoFechas.FechaInicio}','{idClienteYRangoFechas.FechaFin}'";
                var result = await _db.ReporteFactura.FromSqlRaw(sentencia).ToListAsync();

                if (result == null || result.Count == 0)
                {
                    return NotFound("Usted aun no ha emitido órdenes de compra");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
