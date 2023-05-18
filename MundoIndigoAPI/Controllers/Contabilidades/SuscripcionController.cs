using Microsoft.AspNetCore.Mvc;
using Persistencia;
using Persistencia.Interfaces;

namespace MundoIndigoAPI.Controllers.Contabilidades
{
    [Route("api/Suscripcion")]
    [ApiController]
    public class SuscripcionController : ControllerBase
    {
        private readonly IReadDbContext _readDbContext;

        public SuscripcionController(IReadDbContext context)
        {
            _readDbContext = context;
        }

        [Route("ObtenerValorSuscripcion/{idCliente}")]
        [HttpGet]
        public async Task<ActionResult<decimal>> ObtenerValorSuscripcion(string idCliente, CancellationToken cancellationToken)
        {
            try
            {
                idCliente = Dominio.Utilidades.Seguridad.DesEncriptar(idCliente);
                var paramterList = new List<ParameterStored>();

                ParameterStored parame = new()
                {
                    ParameterName = "IdCliente",
                    ParameterValue = idCliente,
                    Type = System.Data.DbType.Int32,
                    Direction = System.Data.ParameterDirection.Input
                };
                paramterList.Add(parame);

                var result = await _readDbContext.ExecuteFirstOrDefaultSpAsync<decimal>("Contabilidades.ObtenerValorSuscripcionPorIdCliente", paramterList, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

