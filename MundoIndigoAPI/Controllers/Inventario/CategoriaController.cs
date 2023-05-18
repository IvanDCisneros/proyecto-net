using Dominio.Inventario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Data;

namespace MundoIndigoAPI.Controllers.Inventario
{
    [Route("api/Categoria")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public CategoriaController(AplicationDBContext db)
        {
            _db = db;
        }

        [Route("GetAll")]
        [HttpGet]
        //public async Task<ActionResult<List<Categoria>>> GetAll()
        public async Task<IActionResult> GetAll()
        {
            var result = await _db.Categorias.Where(c => c.EstaActivo == true).OrderBy(c => c.Nombre).ToListAsync();

            if (result == null)
                return NotFound("Datos no encontrados");

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Categoria categoria)
        {
            var parametroId = new SqlParameter("@idCategoria", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _db.Database.ExecuteSqlInterpolatedAsync(
                $@"EXEC CategoriasInsertar 
                    @nombre={categoria.Nombre}, 
                    @imagen={categoria.RutaImagen},
                    @estaActivo={categoria.EstaActivo},
                    @idCategoria={parametroId} OUTPUT");

            var id = (int)parametroId.Value;
            return Ok(id);
        }
    }
}
