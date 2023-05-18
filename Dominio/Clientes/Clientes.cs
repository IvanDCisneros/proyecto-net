using System.ComponentModel.DataAnnotations;

namespace Dominio.Clientes
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        [Required]
        public Int64 Identificacion { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Direccion { get; set; }
        [Required]
        public int IdDepartamento { get; set; }
        public string? Departamento { get; set; }
        [Required]
        public int IdMunicipio { get; set; }
        public string? Municipio { get; set; }
        [Required]
        public Int64 Telefono { get; set; }
        [Required]
        public string? CorreoElectronico { get; set; }
        [Required]
        public string? Contrasena { get; set; }
        public int IdAfiliado { get; set; }
        public int IdBanco { get; set; }
        public string? Banco { get; set; }
        public Int64 CuentaBancaria { get; set; }
        public string? CodigoAfiliado { get; set; }
        public string? CodigoReferido { get; set; }
        public int IdAfiliadoPadre { get; set; }
    }
}
