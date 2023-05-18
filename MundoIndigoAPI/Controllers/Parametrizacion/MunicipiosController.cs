using Dominio.Parametrizacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace MundoIndigoAPI.Controllers.Parametrizacion
{
    [Route("api/Municipios")]
    [ApiController]
    public class MunicipiosController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public MunicipiosController(AplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet("GetMunicipiosByIdDepartamento/{idDepartamento:int}")]
        public async Task<ActionResult<Municipios>> GetImagenesProductoByIdProducto(int idDepartamento)
        {
            try
            {
                SqlParameter idDepartamentoParameter = new("@idDepartamento", idDepartamento);
                var municipios = (await _db.Municipios.FromSqlRaw($"EXEC Parametrizacion.ObtenerMunicipiosPorIdDepartamento @idDepartamento", idDepartamentoParameter).ToListAsync());

                if (municipios == null)
                    return NotFound();

                return Ok(municipios);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
