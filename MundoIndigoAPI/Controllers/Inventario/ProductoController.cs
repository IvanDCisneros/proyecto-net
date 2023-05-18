using Dominio.Inventario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MundoIndigoAPI.Controllers.Inventario
{
    [Route("api/Producto")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AplicationDBContext _db;

        public ProductoController(AplicationDBContext db)
        {
            _db = db;
        }

        [Route("Get/{idCategoria}")]
        [HttpGet]
        //public async Task<ActionResult<List<Categoria>>> GetAll()
        public async Task<IActionResult> GetProductosPorIdCategoria(int idCategoria)
        {
            var result = idCategoria == 0
                ? await _db.Productos.ToListAsync()
                : await _db.Productos.Where(c => c.IdCategoria == idCategoria).ToListAsync();

            if (result == null)
                return NotFound("Datos no encontrados");

            return Ok(result);
        }

        [HttpGet("GetImagenesProductoByIdProducto/{idProducto:int}")]
        public async Task<ActionResult<ProductoImagenes>> GetImagenesProductoByIdProducto(int idProducto)
        {
            try
            {
                SqlParameter idProductoParameter = new("@idProducto", idProducto);
                var producto = (await _db.ProductoImagenes.FromSqlRaw($"EXEC Inventario.ObtenerImagenesProductoPorIdProducto @idProducto", idProductoParameter).ToListAsync());

                if (producto == null)
                    return NotFound();

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetProductoMercanciaDetalleByIdProducto/{idProducto:int}")]
        public async Task<ActionResult<ProductoMercanciaDetalle>> GetProductoMercanciaDetalleByIdProducto(int idProducto)
        {
            try
            {
                ProductoMercanciaDetalle productoMercanciaDetalle = new();
                SqlParameter idProductoParameter = new("@idProducto", idProducto);

                productoMercanciaDetalle.ListProductoMercancia = await _db.ProductoMercancia.FromSqlRaw($"EXEC Inventario.ObtenerProductoMercanciaPorIdProducto @idProducto", idProductoParameter).ToListAsync();
                productoMercanciaDetalle.ListTalla = await _db.Talla.FromSqlRaw($"EXEC Parametrizacion.ObtenerTallasPorIdProducto @idProducto", idProductoParameter).ToListAsync();
                productoMercanciaDetalle.ListColor = await _db.Color.FromSqlRaw($"EXEC Parametrizacion.ObtenerColoresPorIdProducto @idProducto", idProductoParameter).ToListAsync();
                productoMercanciaDetalle.ListGenero = await _db.Genero.FromSqlRaw($"EXEC Parametrizacion.ObtenerGenerosPorIdProducto @idProducto", idProductoParameter).ToListAsync();

                return Ok(productoMercanciaDetalle);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetListasParametrizacionProducto")]
        public async Task<ActionResult<ListasParametrizacionProducto>> GetListasParametrizacionProducto()
        {
            try
            {
                ListasParametrizacionProducto listasParametrizacionProducto = new();

                listasParametrizacionProducto.ListCategoria = await _db.Categorias.Where(c => c.EstaActivo == true).OrderBy(c => c.Nombre).ToListAsync();
                listasParametrizacionProducto.ListColor = await _db.Color.OrderBy(c => c.Nombre).ToListAsync();
                listasParametrizacionProducto.ListTalla = await _db.Talla.OrderBy(c => c.Nombre).ToListAsync();
                listasParametrizacionProducto.ListGenero = await _db.Genero.OrderBy(c => c.Nombre).ToListAsync();

                return Ok(listasParametrizacionProducto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetProductosMercanciaByIdCategoria/{idCategoria:int}")]
        public async Task<ActionResult<ProductoMercancia>> GetProductosMercanciaByIdCategoria(int idCategoria)
        {
            try
            {
                SqlParameter idCategoriaParameter = new("@idCategoria", idCategoria);

                var productoMercanciaDB = await _db.ProductoMercancia.FromSqlRaw($"EXEC Inventario.ObtenerProductosMercanciaPorIdCategoria @idCategoria", idCategoriaParameter).ToListAsync();
                return Ok(productoMercanciaDB);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetProductoMercanciaById/{idProductoMercancia:int}")]
        public async Task<ActionResult<ProductoMercancia>> GetProductoMercanciaById(int idProductoMercancia)
        {
            try
            {
                SqlParameter idProductoMercanciaParameter = new("@idProductoMercancia", idProductoMercancia);
                var productoMercanciaDB = await _db.ProductoMercancia.FromSqlRaw($"EXEC Inventario.ObtenerProductoMercanciaPorId @idProductoMercancia", idProductoMercanciaParameter).ToListAsync();
                return productoMercanciaDB[0];
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("GetProductoPorIdCategoriaPorSP/{idCategoria:int}")]
        public async Task<ActionResult<Producto>> GetProductoPorIdCategoriaPorSP(int idCategoria)
        {
            try
            {
                SqlParameter idCategoriaParameter = new("@idCategoria", idCategoria);
                var productos = (await _db.Productos.FromSqlRaw($"EXEC Inventario.ObtenerProductosPorIdCategoria @idCategoria", idCategoriaParameter).ToListAsync());

                if (productos == null)
                    return NotFound("Datos no encontrados");

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("ActualizarProductoMercancia")]
        [HttpPost]
        public async Task<ActionResult<int>> ActualizarProductoMercancia([FromBody] ProductoMercancia productoMercancia)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("IdProducto", typeof(int));
                dt.Columns.Add("IdProductoMercancia", typeof(int));
                dt.Columns.Add("NombreProducto", typeof(string));
                dt.Columns.Add("Descripcion", typeof(string));
                dt.Columns.Add("IdCategoria", typeof(int));
                dt.Columns.Add("RutaImagen", typeof(string));
                dt.Columns.Add("IdGenero", typeof(int));
                dt.Columns.Add("IdColor", typeof(int));
                dt.Columns.Add("IdTalla", typeof(int));
                dt.Columns.Add("Existencia", typeof(string));
                dt.Columns.Add("ValorCosto", typeof(decimal));
                dt.Columns.Add("ValorVenta", typeof(decimal));
                dt.Columns.Add("IdUsuario", typeof(int));

                dt.Rows.Add(
                    productoMercancia.IdProducto,
                    productoMercancia.IdProductoMercancia,
                    productoMercancia.NombreProducto,
                    productoMercancia.Descripcion,
                    productoMercancia.IdCategoria,
                    productoMercancia.RutaImagen,
                    productoMercancia.IdGenero,
                    productoMercancia.IdColor,
                    productoMercancia.IdTalla,
                    productoMercancia.Existencia,
                    productoMercancia.ValorCosto,
                    productoMercancia.ValorVenta,
                    Dominio.Utilidades.Seguridad.DesEncriptar(productoMercancia.IdUsuario ?? ""));

                var parametros = new SqlParameter("@datosProductoMercancia", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Inventario.DatosProductoMercancia"
                };

                var productoMercanciaBD = (await _db.ProductoMercancia.FromSqlRaw($"EXEC Inventario.ActualizarProductoMercancia @datosProductoMercancia", parametros).ToListAsync());

                if (productoMercanciaBD == null || productoMercanciaBD.Count == 0)
                    return NotFound("El producto mercancia aun no ha sido registrado");

                return 1;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("CrearProductoMercancia")]
        [HttpPost]
        public async Task<ActionResult<int>> CrearProductoMercancia([FromBody] ProductoMercancia productoMercancia)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("IdProducto", typeof(int));
                dt.Columns.Add("IdProductoMercancia", typeof(int));
                dt.Columns.Add("NombreProducto", typeof(string));
                dt.Columns.Add("Descripcion", typeof(string));
                dt.Columns.Add("IdCategoria", typeof(int));
                dt.Columns.Add("RutaImagen", typeof(string));
                dt.Columns.Add("IdGenero", typeof(int));
                dt.Columns.Add("IdColor", typeof(int));
                dt.Columns.Add("IdTalla", typeof(int));
                dt.Columns.Add("Existencia", typeof(string));
                dt.Columns.Add("ValorCosto", typeof(decimal));
                dt.Columns.Add("ValorVenta", typeof(decimal));
                dt.Columns.Add("IdUsuario", typeof(int));

                dt.Rows.Add(
                    productoMercancia.IdProducto,
                    productoMercancia.IdProductoMercancia,
                    productoMercancia.NombreProducto,
                    productoMercancia.Descripcion,
                    productoMercancia.IdCategoria,
                    productoMercancia.RutaImagen,
                    productoMercancia.IdGenero,
                    productoMercancia.IdColor,
                    productoMercancia.IdTalla,
                    productoMercancia.Existencia,
                    productoMercancia.ValorCosto,
                    productoMercancia.ValorVenta,
                    Dominio.Utilidades.Seguridad.DesEncriptar(productoMercancia.IdUsuario ?? ""));

                var parametros = new SqlParameter("@datosProductoMercancia", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Inventario.DatosProductoMercancia"
                };

                var productoMercanciaBD = (await _db.ProductoMercancia.FromSqlRaw($"EXEC Inventario.CrearProductoMercancia @datosProductoMercancia", parametros).ToListAsync());

                if (productoMercanciaBD == null || productoMercanciaBD.Count == 0)
                    return NotFound("El producto mercancia aun no ha sido registrado");

                return 1;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
