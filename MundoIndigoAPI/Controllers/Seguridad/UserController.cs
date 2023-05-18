using Dominio.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MundoIndigoAPI.Services;
using Persistencia;

namespace MundoIndigoAPI.Controllers.Seguridad
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AplicationDBContext _db;
        private readonly IConfiguration _config;

        public UserController(AplicationDBContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        [Route("Authenticate")]
        [HttpPost]
        public async Task<ActionResult<TokenJWT>> Authenticate(Login login)
        {
            try
            {
                string token = "";
                UserServices userServices = new(_config);
                login.Password = Dominio.Utilidades.Encrypt.GetSHA256(login.Password);
                string sentencia = $"EXEC Seguridad.ObtenerUsuarioIniciandoSesion '{login.Usuario}', '{login.Password}'";
                var usuarioBD = await _db.Usuarios.FromSqlRaw(sentencia).ToListAsync();

                if (usuarioBD == null)
                {
                    NotFound("El Usuario o cliente aun no ha sido registrado");
                }
                else
                {
                    token = userServices.Authenticate(Dominio.Utilidades.Seguridad.Encriptar(usuarioBD[0].IdUsuario.ToString()), usuarioBD[0].NombreRol);
                }

                TokenJWT tokenJWT = new TokenJWT();
                tokenJWT.Token = token;

                return tokenJWT;

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
