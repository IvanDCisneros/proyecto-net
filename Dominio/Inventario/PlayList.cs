using System.ComponentModel.DataAnnotations;

namespace Dominio.Inventario
{
    public class PlayList
    {
        [Key]
        public int IdPlayList { get; set; }
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string? NombreCancion { get; set; }
        public string? NombreArtista { get; set; }
        public string? UrlImagen { get; set; }
        public string? UrlCancion { get; set; }
    }
}
