using Dominio.Inventario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Data;

namespace MundoIndigoAPI.Controllers.Inventario
{
    [Route("api/PlayList")]
    [ApiController]
    public class PlayListController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public PlayListController(AplicationDBContext db)
        {
            _db = db;
        }


        [Route("ObtenerPlayListPorIdCategorias")]
        [HttpPost]
        public async Task<ActionResult<PlayList>> ObtenerPlayListPorIdCategorias(List<Ids> listaIdsCategorias)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));

                foreach (Ids item in listaIdsCategorias)
                {
                    dt.Rows.Add(item.Id);
                }

                var parametros = new SqlParameter("@ids", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Inventario.Ids"
                };

                var result = await _db.PlayList.FromSqlRaw($"EXEC Inventario.ObtenerPlayListPorIdCategorias @ids", parametros).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
