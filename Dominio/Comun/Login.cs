using System.ComponentModel.DataAnnotations;

namespace Dominio.Clientes
{
    public class Login
    {
        [Required(ErrorMessage = "El usuario es obligatorio. Puede ingresar su identificación, correo electrónico o código de afiliado")]
        public string? Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Password { get; set; }
    }
}
