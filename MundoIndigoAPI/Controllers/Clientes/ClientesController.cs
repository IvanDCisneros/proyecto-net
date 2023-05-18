using Dominio.Clientes;
using Dominio.Notificaciones;
using Dominio.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MundoIndigoAPI.Services;
using Persistencia;
using Persistencia.Interfaces;
using System.Data;

namespace MundoIndigoAPI.Controllers.Clientes
{
    [Route("api/Clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AplicationDBContext _db;
        private readonly IConfiguration _config;
        private readonly IReadDbContext _readDbContext;
        private readonly IOptions<App> _options;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ClientesController(AplicationDBContext db, IConfiguration config, IReadDbContext context, IOptions<App> options, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _config = config;
            _readDbContext = context;
            _options = options;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("Authenticate")]
        [HttpPost]
        public async Task<ActionResult<ClienteAutenticado>> Authenticate(Login login)
        {
            try
            {
                string token = "";
                UserServices userServices = new(_config);
                login.Password = Dominio.Utilidades.Encrypt.GetSHA256(login.Password ?? "");
                string sentencia = $"EXEC Clientes.ObtenerClienteIniciandoSesion '{login.Usuario}', '{login.Password}'";
                var clienteBD = await _db.Clientes.FromSqlRaw(sentencia).ToListAsync();

                if (clienteBD == null)
                {
                    NotFound("El Usuario o cliente aun no ha sido registrado");
                }
                else
                {
                    token = userServices.Authenticate(Dominio.Utilidades.Seguridad.Encriptar(clienteBD[0].IdCliente.ToString()), "");
                }

                ClienteAutenticado clienteAutenticado = new();
                clienteAutenticado.CodigoAfiliado = clienteBD?[0].CodigoAfiliado;
                clienteAutenticado.Token = token;

                return clienteAutenticado;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("CreacionCliente")]
        [HttpPost]
        public async Task<ActionResult<ClienteAutenticado>> CreacionCliente([FromBody] Cliente cliente)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("IdCliente", typeof(int));
                dt.Columns.Add("Identificacion", typeof(Int64));
                dt.Columns.Add("Nombre", typeof(string));
                dt.Columns.Add("Direccion", typeof(string));
                dt.Columns.Add("IdDepartamento", typeof(int));
                dt.Columns.Add("IdMunicipio", typeof(int));
                dt.Columns.Add("Telefono", typeof(Int64));
                dt.Columns.Add("CorreoElectronico", typeof(string));
                dt.Columns.Add("Contraseña", typeof(string));
                dt.Columns.Add("IdBanco", typeof(int));
                dt.Columns.Add("CuentaBancaria", typeof(Int64));
                dt.Columns.Add("CodigoReferido", typeof(string));

                dt.Rows.Add(
                    cliente.IdCliente,
                    cliente.Identificacion,
                    cliente.Nombre,
                    cliente.Direccion,
                    cliente.IdDepartamento,
                    cliente.IdMunicipio,
                    cliente.Telefono,
                    cliente.CorreoElectronico,
                    Encrypt.GetSHA256(cliente.Contrasena ?? ""),
                    cliente.IdBanco,
                    cliente.CuentaBancaria,
                    cliente.CodigoReferido);

                var parametros = new SqlParameter("@datosCliente", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Clientes.DatosCliente"
                };

                var clienteBD = await _db.Clientes.FromSqlRaw($"EXEC Clientes.InsertarNuevoCliente @datosCliente", parametros).ToListAsync();

                if (clienteBD == null)
                    return NotFound("El cliente aun no ha sido registrado");

                UserServices userServices = new(_config);
                ClienteAutenticado clienteAutenticado = new();
                clienteAutenticado.CodigoAfiliado = clienteBD[0].CodigoAfiliado;
                clienteAutenticado.Token = userServices.Authenticate(Dominio.Utilidades.Seguridad.Encriptar(clienteBD[0].IdCliente.ToString()), "");

                Mail.From = _options.Value.EmailSettings?.Sender;
                Mail.Host = _options.Value.EmailSettings?.MailServer;
                Mail.PassWord = _options.Value.EmailSettings?.Password;
                Mail.Port = _options.Value.EmailSettings!.MailPort;
                Mail.ActivoSsl = _options.Value.EmailSettings.Activar;

                string Urlplantilla = "";
                var payloadPlantilla = "";
                string dataEmail = "";

                if (clienteBD[0].IdBanco != 0)
                {
                    Urlplantilla = $"{_webHostEnvironment.ContentRootPath}/PlantillasEmail/NotificacionCreacionAfiliado.html";
                    payloadPlantilla = System.IO.File.ReadAllText(Urlplantilla);
                    dataEmail = PlantillasCorreoUtil.MergeInfoPlantilla(clienteBD[0], payloadPlantilla);
                }
                else
                {
                    Urlplantilla = $"{_webHostEnvironment.ContentRootPath}/PlantillasEmail/NotificacionCreacionUsuario.html";
                    payloadPlantilla = System.IO.File.ReadAllText(Urlplantilla);
                    dataEmail = PlantillasCorreoUtil.MergeInfoPlantilla(clienteBD[0], payloadPlantilla);
                }
                                
                string Subject = "Notificación creación de usuario";
                await Mail.EnviarMail(clienteBD[0].CorreoElectronico ?? "", Subject, dataEmail);

                return clienteAutenticado;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("ActualizarCliente")]
        [HttpPost]
        public async Task<ActionResult<ClienteAutenticado>> ActualizarCliente([FromBody] Cliente cliente)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("IdCliente", typeof(int));
                dt.Columns.Add("Identificacion", typeof(Int64));
                dt.Columns.Add("Nombre", typeof(string));
                dt.Columns.Add("Direccion", typeof(string));
                dt.Columns.Add("IdDepartamento", typeof(int));
                dt.Columns.Add("IdMunicipio", typeof(int));
                dt.Columns.Add("Telefono", typeof(Int64));
                dt.Columns.Add("CorreoElectronico", typeof(string));
                dt.Columns.Add("Contraseña", typeof(string));
                dt.Columns.Add("IdBanco", typeof(int));
                dt.Columns.Add("CuentaBancaria", typeof(Int64));
                dt.Columns.Add("CodigoReferido", typeof(string));

                dt.Rows.Add(
                    cliente.IdCliente,
                    cliente.Identificacion,
                    cliente.Nombre,
                    cliente.Direccion,
                    cliente.IdDepartamento,
                    cliente.IdMunicipio,
                    cliente.Telefono,
                    cliente.CorreoElectronico,
                    Encrypt.GetSHA256(cliente.Contrasena ?? ""),
                    cliente.IdBanco,
                    cliente.CuentaBancaria,
                    cliente.CodigoReferido);

                var parametros = new SqlParameter("@datosCliente", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Clientes.DatosCliente"
                };

                var clienteBD = (await _db.Clientes.FromSqlRaw($"EXEC Clientes.ActualizarCliente @datosCliente", parametros).ToListAsync());

                if (clienteBD == null)
                    return NotFound("El cliente aun no ha sido registrado");

                UserServices userServices = new(_config);
                ClienteAutenticado clienteAutenticado = new();
                clienteAutenticado.CodigoAfiliado = clienteBD[0].CodigoAfiliado;
                clienteAutenticado.Token = userServices.Authenticate(Dominio.Utilidades.Seguridad.Encriptar(clienteBD[0].IdCliente.ToString()), "");

                Mail.From = _options.Value.EmailSettings?.Sender;
                Mail.Host = _options.Value.EmailSettings?.MailServer;
                Mail.PassWord = _options.Value.EmailSettings?.Password;
                Mail.Port = _options.Value.EmailSettings!.MailPort;
                Mail.ActivoSsl = _options.Value.EmailSettings.Activar;

                string Urlplantilla = "";
                var payloadPlantilla = "";
                string dataEmail = "";

                if (clienteBD[0].IdBanco != 0)
                {
                    Urlplantilla = $"{_webHostEnvironment.ContentRootPath}/PlantillasEmail/NotificacionActualizacionAfiliado.html";
                    payloadPlantilla = System.IO.File.ReadAllText(Urlplantilla);
                    dataEmail = PlantillasCorreoUtil.MergeInfoPlantilla(clienteBD[0], payloadPlantilla);
                }
                else
                {
                    Urlplantilla = $"{_webHostEnvironment.ContentRootPath}/PlantillasEmail/NotificacionActualizacionUsuario.html";
                    payloadPlantilla = System.IO.File.ReadAllText(Urlplantilla);
                    dataEmail = PlantillasCorreoUtil.MergeInfoPlantilla(clienteBD[0], payloadPlantilla);
                }

                string Subject = "Notificación actualización de usuario";
                await Mail.EnviarMail(clienteBD[0].CorreoElectronico ?? "", Subject, dataEmail);

                return clienteAutenticado;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("ObtenerCliente/{idCliente}")]
        [HttpGet]
        public async Task<ActionResult<Cliente>> ObtenerCliente(string idCliente)
        {
            try
            {
                idCliente = Dominio.Utilidades.Seguridad.DesEncriptar(idCliente); 
                SqlParameter idClienteParameter = new("@idCliente", idCliente);
                var result = await _db.Clientes.FromSqlRaw($"EXEC Clientes.ObtenerClientePorIdCliente @idCliente", idClienteParameter).ToListAsync();
                return result[0];
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("RecuperarPassword/{usuario}")]
        [HttpGet]
        public async Task<ActionResult<int>> RecuperarPassword(string usuario, CancellationToken cancellationToken)
        {
            try
            {
                var paramterList = new List<ParameterStored>();

                ParameterStored parame = new()
                {
                    ParameterName = "usuario",
                    ParameterValue = usuario,
                    Type = System.Data.DbType.String,
                    Direction = System.Data.ParameterDirection.Input
                };
                paramterList.Add(parame);

                var result = await _readDbContext.ExecuteFirstOrDefaultSpAsync<string>("Clientes.RecuperarPassword", paramterList, cancellationToken);
                string[] words = result.Split('/');

                Mail.From = _options.Value.EmailSettings?.Sender;
                Mail.Host = _options.Value.EmailSettings?.MailServer;
                Mail.PassWord = _options.Value.EmailSettings?.Password;
                Mail.Port = _options.Value.EmailSettings!.MailPort;
                Mail.ActivoSsl = _options.Value.EmailSettings.Activar;

                string Urlplantilla = $"{_webHostEnvironment.ContentRootPath}/PlantillasEmail/NotificacionCambioPassword.html";
                var payloadPlantilla = System.IO.File.ReadAllText(Urlplantilla);

                NotificacionCambioPassword data = new()
                {
                    Password = words[2]
                };

                string dataEmail = PlantillasCorreoUtil.MergeInfoPlantilla(data, payloadPlantilla);
                string Subject = "Notificación cambio de contraseña";
                
                var dt = new DataTable();
                dt.Columns.Add("IdCliente", typeof(int));
                dt.Columns.Add("Contraseña", typeof(string));
                
                dt.Rows.Add(words[1], Encrypt.GetSHA256(words[2]));

                var parametros = new SqlParameter("@datosCambioPassword", SqlDbType.Structured)
                {
                    Value = dt,
                    TypeName = "Clientes.DatosCambioPassword"
                };

                await _db.Clientes.FromSqlRaw($"EXEC Clientes.CambioPasswordCliente @datosCambioPassword", parametros).ToListAsync();
                await Mail.EnviarMail(words[0], Subject, dataEmail);

                return 1;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
