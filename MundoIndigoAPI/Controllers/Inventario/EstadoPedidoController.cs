using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace MundoIndigoAPI.Controllers.Parametrizacion
{
    [Route("api/EstadoPedido")]
    [ApiController]
    public class EstadoPedidoController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public EstadoPedidoController(AplicationDBContext db)
        {
            _db = db;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _db.EstadoPedidos.ToListAsync();

            if (result == null)
                return NotFound("Datos no encontrados");

            return Ok(result);
        }
    }
}
