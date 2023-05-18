using Dominio.Inventario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Data;

namespace MundoIndigoAPI.Controllers.Inventario
{
    [Route("api/Pedidos")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public PedidoController(AplicationDBContext db)
        {
            _db = db;
        }

        [Route("ObtenerPedidos")]
        [HttpPost]
        public async Task<ActionResult<Pedido>> ObtenerPedidos(IdEstadoPedidoYRangoFechas idEstadoPedidoYRangoFechas)
        {
            try
            {
                string sentencia = $"EXEC Inventario.ObtenerPedidosPorIdEstadoPedidoYRangoFechas '{idEstadoPedidoYRangoFechas.IdEstadoPedido}','{idEstadoPedidoYRangoFechas.FechaInicio}','{idEstadoPedidoYRangoFechas.FechaFin}'";
                var result = await _db.Pedidos.FromSqlRaw(sentencia).ToListAsync();

                if (result == null || result.Count == 0)
                {
                    return NotFound("No existen pedidos para los filtros seleccionados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("ObtenerPedidoPorId/{idPedido}")]
        [HttpGet]
        public async Task<ActionResult<Pedido>> ObtenerPedidoPorId(int idPedido)
        {
            try
            {
                string sentencia = $"EXEC Inventario.ObtenerPedidoPorId '{idPedido}'";
                var result = await _db.Pedidos.FromSqlRaw(sentencia).ToListAsync();

                if (result == null || result.Count == 0)
                {
                    return NotFound("No existe el pedido");
                }

                return result[0];
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("GuardarPedido")]
        [HttpPost]
        public async Task<ActionResult<int>> GuardarPedido([FromBody] Pedido pedido)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("IdPedido", typeof(int));
                dt.Columns.Add("numeroGuia", typeof(Int64));
                dt.Columns.Add("empresaEnvio", typeof(string));
                dt.Columns.Add("IdEstadoPedido", typeof(int));
                dt.Columns.Add("IdUsuario", typeof(int));

                dt.Rows.Add(
                    pedido.IdPedido,
                    pedido.NumeroGuia,
                    pedido.EmpresaEnvio,
                    pedido.IdEstadoPedido,
                    Dominio.Utilidades.Seguridad.DesEncriptar(pedido.IdUsuario ?? ""));

                var parametros = new SqlParameter("@datosPedido", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Inventario.DatosPedido"
                };

                var pedidoBD = (await _db.Pedidos.FromSqlRaw($"EXEC Inventario.GuardarPedido @datosPedido", parametros).ToListAsync());

                if (pedidoBD == null)
                    return NotFound("El pedido aun no ha sido registrado");

                return 1;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
