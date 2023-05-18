using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Inventario
{
    public class ProductoImagenes
    {
        public Int64 Id { get; set; }
        [Required]
        public string? RutaImagen { get; set; }

    }
}
