using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace MundoIndigoAPI.Controllers.Parametrizacion
{
    [Route("api/Banco")]
    [ApiController]
    public class BancoController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public BancoController(AplicationDBContext db)
        {
            _db = db;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _db.Bancos.ToListAsync();

            if (result == null)
                return NotFound("Datos no encontrados");

            return Ok(result);
        }
    }
}
