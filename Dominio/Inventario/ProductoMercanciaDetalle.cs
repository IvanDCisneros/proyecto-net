using Dominio.Parametrizacion;
using System.Collections.Generic;
using System.Drawing;

namespace Dominio.Inventario
{
    public class ProductoMercanciaDetalle
    {
        public List<ProductoMercancia>? ListProductoMercancia { get; set; }
        public List<Talla>? ListTalla { get; set; }
        public List<Genero>? ListGenero { get; set; }
        public List<Parametrizacion.Color>? ListColor { get; set; }

    }
}
