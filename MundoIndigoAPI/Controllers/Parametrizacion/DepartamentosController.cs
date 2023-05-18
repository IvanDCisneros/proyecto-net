using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace MundoIndigoAPI.Controllers.Parametrizacion
{
    [Route("api/Departamentos")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public DepartamentosController(AplicationDBContext db)
        {
            _db = db;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _db.Departamentos.ToListAsync();

            if (result == null)
                return NotFound("Datos no encontrados");

            return Ok(result);
        }
    }
}
