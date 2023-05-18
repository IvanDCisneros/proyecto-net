using Dominio.Contabilidades;
using Dominio.Inventario;
using Dominio.Notificaciones;
using Dominio.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistencia;
using Persistencia.Interfaces;
using System.Data;

namespace MundoIndigoAPI.Controllers.Contabilidades
{
    [Route("api/Factura")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly AplicationDBContext _db;
        private readonly IReadDbContext _readDbContext;
        private readonly IOptions<App> _options;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FacturaController(AplicationDBContext db, IReadDbContext context, IOptions<App> options, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _readDbContext = context;
            _options = options;
            _webHostEnvironment = webHostEnvironment;
        }

        //[Authorize]
        [Route("CreacionFactura")]
        [HttpPost]
        public async Task<ActionResult<Factura>> CreacionFactura(List<DatosFactura> listDatosFactura)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("IdCliente", typeof(int));
                dt.Columns.Add("IdProductoMercancia", typeof(int));
                dt.Columns.Add("ValorVenta", typeof(decimal));
                dt.Columns.Add("Cantidad", typeof(int));
                dt.Columns.Add("SubTotal", typeof(decimal));

                foreach (DatosFactura item in listDatosFactura)
                {
                    dt.Rows.Add(
                        Dominio.Utilidades.Seguridad.DesEncriptar(item.IdCliente ?? ""),
                        item.IdProductoMercancia,
                        item.ValorVenta,
                        item.Cantidad,
                        item.SubTotal);
                }

                var parametros = new SqlParameter("@nuevaFactura", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Contabilidades.DatosParaFactura"
                };

                var facturaBD = await _db.Factura.FromSqlRaw($"EXEC Contabilidades.CreacionNuevaFactura @nuevaFactura", parametros).ToListAsync();

                if (facturaBD.Count == 0)
                {
                    return NotFound("Lo sentimos!!, No logramos realizar la orden de compra por favor comuníquese con las líneas activas de Mundo Índigo.");
                }

                SqlParameter idFacturaParameter = new("@idFactura", facturaBD[0].IdFactura);

                facturaBD[0].ListItemsFacturas = await _db.ItemsFactura.FromSqlRaw($"EXEC Contabilidades.ObtenerItemsFacturaPorIdFactura @idFactura", idFacturaParameter).ToListAsync();
                return facturaBD[0];
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("PagarFactura/{idFactura}")]
        [HttpGet]
        public async Task<ActionResult<int>> PagarFactura(int idFactura, CancellationToken cancellationToken)
        {
            try
            {
                var paramterList = new List<ParameterStored>();

                ParameterStored parame = new()
                {
                    ParameterName = "IdFactura",
                    ParameterValue = idFactura,
                    Type = System.Data.DbType.Int32,
                    Direction = System.Data.ParameterDirection.Input
                };
                paramterList.Add(parame);

                var result = await _readDbContext.ExecuteFirstOrDefaultSpAsync<int>("Contabilidades.GeneracionComprobanteContable", paramterList, cancellationToken);

                SqlParameter idFacturaParameter = new("@idFactura", result);
                var resultFactura = await _db.NotificacionCompra.FromSqlRaw($"EXEC Clientes.DatosNotificacionCompraCliente @idFactura", idFacturaParameter).ToListAsync();

                Mail.From = _options.Value.EmailSettings?.Sender;
                Mail.Host = _options.Value.EmailSettings?.MailServer;
                Mail.PassWord = _options.Value.EmailSettings?.Password;
                Mail.Port = _options.Value.EmailSettings!.MailPort;
                Mail.ActivoSsl = _options.Value.EmailSettings!.Activar;

                string Urlplantilla = $"{_webHostEnvironment.ContentRootPath}/PlantillasEmail/NotificacionCompra.html";
                var payloadPlantilla = System.IO.File.ReadAllText(Urlplantilla);

                NotificacionCompra data = new NotificacionCompra();
                data.Identificacion = resultFactura[0].Identificacion;
                data.Nombre = resultFactura[0].Nombre;
                data.CorreoElectronico = resultFactura[0].CorreoElectronico;
                data.Fecha = resultFactura[0].Fecha;
                data.NumeroFactura = resultFactura[0].NumeroFactura;
                data.ValorTotal = resultFactura[0].ValorTotal;
                data.Direccion = resultFactura[0].Direccion;
                data.Municipio = resultFactura[0].Municipio;
                data.Departamento = resultFactura[0].Departamento;

                string dataEmail = PlantillasCorreoUtil.MergeInfoPlantilla(data, payloadPlantilla);
                string Subject = "Notificación compra realizada";
                await Mail.EnviarMail(resultFactura[0].CorreoElectronico ?? "", Subject, dataEmail);

                return result;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //[Authorize]
        [Route("ActualizarCarrito")]
        [HttpPost]
        public async Task<ActionResult<List<CartItem>>> ActualizarCarrito(List<CartItem> listCartItem)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("IdProductoMercancia", typeof(int));
                dt.Columns.Add("NombreProducto", typeof(string));
                dt.Columns.Add("NombreGenero", typeof(string));
                dt.Columns.Add("NombreTalla", typeof(string));
                dt.Columns.Add("NombreColor", typeof(string));
                dt.Columns.Add("ExistenciasBodega", typeof(int));
                dt.Columns.Add("ValorVenta", typeof(decimal));
                dt.Columns.Add("Cantidad", typeof(int));
                dt.Columns.Add("SubTotal", typeof(decimal));

                foreach (CartItem item in listCartItem)
                {
                    dt.Rows.Add(
                        item.IdProductoMercancia,
                        item.NombreProducto,
                        item.NombreGenero,
                        item.NombreTalla,
                        item.NombreColor,
                        item.ExistenciasBodega,
                        item.ValorVenta,
                        item.Cantidad,
                        item.SubTotal);
                }

                var parametros = new SqlParameter("@actualizarCarrito", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Inventario.ItemsCarrito"
                };

                var cartItemsBD = await _db.CartItems.FromSqlRaw($"EXEC Inventario.ActualizarCarritoDeCompras @actualizarCarrito", parametros).ToListAsync();

                return cartItemsBD;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

