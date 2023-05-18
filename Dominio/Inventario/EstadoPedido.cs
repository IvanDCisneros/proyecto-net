using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Parametrizacion
{
    [Table("EstadoPedido", Schema = "Inventario")]
    public class EstadoPedido
    {
        [Key]
        public int IdEstadoPedido { get; set; }
        public string? Nombre { get; set; }
    }
}
