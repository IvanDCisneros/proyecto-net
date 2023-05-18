using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Notificaciones
{
    public class NotificacionCambioPassword
    {
        [Key]
        public string? Password { get; set; }
    }
}
