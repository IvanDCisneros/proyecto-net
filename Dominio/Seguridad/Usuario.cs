using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Seguridad
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required]
        public Int64 Identificacion { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? CorreoElectronico { get; set; }
        [Required]
        public int IdRol { get; set; }
        [Required]
        public string? NombreRol { get; set; }
        [Required]
        public bool EstaActivo { get; set; }

    }
}
