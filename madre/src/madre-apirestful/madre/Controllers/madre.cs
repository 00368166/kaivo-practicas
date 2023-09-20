using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;


namespace madre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class madre : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IConfiguration Configuration;
        private readonly HttpClient _httpClient;

        public madre(IConfiguration configuration)
        {
            _configuration = configuration;
            Configuration = configuration;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public ActionResult<string> Saludar()
        {
            return Ok("Bienvenido al servicio de la madre");
        }

        [HttpPost]
        public IActionResult CrearDato([FromBody] DatoModelo dato)
        {
            // Crear el nuevo dato utilizando la información recibida en el cuerpo de la solicitud
            // ...

            return Ok("Dato creado exitosamente");
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarDato(int id)
        {
            // Eliminar el dato con el ID especificado
            // ...

            return Ok("Dato eliminado exitosamente");
        }

        [HttpPost("ejecutar-programa")]
        public IActionResult EjecutarPrograma()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    process.WaitForInputIdle();

                    var processId = process.Id;

                    return Ok(new
                    {
                        ProcessId = processId,
                        Message = $"El programa Notepad se ha iniciado correctamente con el PID {processId}."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"Error al ejecutar el programa: {ex.Message}"
                });
            }
        }

        [HttpDelete("cerrar-programa/{pid}")]
        public IActionResult CerrarPrograma(int pid)
        {
            try
            {
                var process = Process.GetProcessById(pid);
                var processName = process.ProcessName;
                var uptime = DateTime.Now - process.StartTime;

                process.Kill();

                var response = new
                {
                    Message = $"Programa '{processName}' cerrado exitosamente",
                    Uptime = uptime
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = $"Error al cerrar el programa: {ex.Message}" });
            }
        }

        [HttpGet("hijas")]
        public IActionResult ObtenerHijas()
        {
            try
            {
                using (var connection = new MySqlConnection(_configuration.GetConnectionString("MiConexion")))
                {
                    connection.Open();

                    var query = "SELECT Ip, Nombre, Status, Permiso FROM computadorashijas";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var hijas = new List<ComputadoraHija>();
                            while (reader.Read())
                            {
                                var ip = reader.GetString(0);
                                var nombre = reader.GetString(1);
                                var status = reader.GetString(2);
                                var permiso = reader.GetString(3);

                                var hija = new ComputadoraHija
                                {
                                    Ip = ip,
                                    Nombre = nombre,
                                    Status = status,
                                    Permiso = permiso
                                };

                                hijas.Add(hija);
                            }

                            return Ok(hijas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"Error al obtener las computadoras hijas: {ex.Message}"
                });
            }
        }

        [HttpGet("verificar-ip/{ip}")]
        public async Task<IActionResult> VerificarIP(string ip)
        {
            try
            {
                string url = $"http://{ip}/api/hija";

                // Realizar la solicitud GET a la IP especificada
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // La conexión fue exitosa, actualizar el estado a 1 en la base de datos
                    await ActualizarEstadoHija(ip, 1);

                    return Ok($"La conexión con la IP {ip} es exitosa.");
                }
                else
                {
                    // La conexión no fue exitosa, actualizar el estado a 0 en la base de datos
                    await ActualizarEstadoHija(ip, 0);

                    return Ok($"No se pudo establecer la conexión con la IP {ip}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Error al verificar la conexión.",
                    Error = ex.Message
                });
            }
        }

        private async Task ActualizarEstadoHija(string ip, int estado)
        {
            try
            {
                using (var connection = new MySqlConnection(_configuration.GetConnectionString("MiConexion")))
                {
                    connection.Open();

                    var query = "UPDATE computadorashijas SET Status = @Estado WHERE Ip = @Ip";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Estado", estado);
                        command.Parameters.AddWithValue("@Ip", ip);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error en caso de que no se pueda actualizar el estado
                Console.WriteLine($"Error al actualizar el estado de la hija con IP {ip}: {ex.Message}");
            }
        }

        [HttpPost("apagar/{ip}")]
        public async Task<IActionResult> Apagar(string ip)
        {
            try
            {
                string url = $"http://{ip}/api/hija/apagar";

                // Realizar la solicitud POST a la IP especificada para apagar la hija
                HttpResponseMessage response = await _httpClient.PostAsync(url, null);

                // Actualizar el estado y el permiso en la base de datos sin esperar respuesta de la hija
                await ActualizarEstadoHija(ip, 0);
                await ActualizarPermisoHija(ip, 0);

                return Ok($"Se ha enviado la solicitud de apagado a la IP {ip}.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Error al enviar la solicitud de apagado.",
                    Error = ex.Message
                });
            }
        }

        private async Task ActualizarPermisoHija(string ip, int permiso)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("MiConexion")))
            {
                await connection.OpenAsync();

                var query = "UPDATE computadorashijas SET Permiso = @Permiso WHERE Ip = @Ip";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Permiso", permiso);
                    command.Parameters.AddWithValue("@Ip", ip);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        [HttpPost("habilitar/{ip}")]
        public async Task<IActionResult> Habilitar(string ip)
        {
            var estado = await ObtenerEstadoHija(ip);

            if (estado != 1)
            {
                return BadRequest($"No está permitido habilitar. Status actual: {estado}");
            }
            else
            {
                using (var connection = new MySqlConnection(_configuration.GetConnectionString("MiConexion")))
                {
                    await connection.OpenAsync();

                    var query = "UPDATE computadorashijas SET Permiso = @Permiso WHERE Ip = @Ip";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Permiso", 1); // Cambiar a 1 para habilitado
                        command.Parameters.AddWithValue("@Ip", ip);

                        var rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return Ok("Se pudo habilitar");
                        }
                        else
                        {
                            return BadRequest("No se pudo habilitar");
                        }
                    }
                }
            }
        }





        private async Task<int> ObtenerEstadoHija(string ip)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("MiConexion")))
            {
                await connection.OpenAsync();

                var query = "SELECT Status FROM computadorashijas WHERE Ip = @Ip";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ip", ip);

                    var result = await command.ExecuteScalarAsync();

                    if (result != null && bool.TryParse(result.ToString(), out bool estado))
                    {
                        return estado ? 1 : 0;
                    }
                }
            }

            // Si no se encuentra el estado en la base de datos o hay algún error, se devuelve el estado 0
            return 2;
        }


        [HttpPost("inhabilitar/{ip}")]
        public async Task<IActionResult> Inhabilitar(string ip)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("MiConexion")))
            {
                await connection.OpenAsync();

                var query = "UPDATE computadorashijas SET Permiso = @Permiso WHERE Ip = @Ip";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Permiso", 0); // Cambiar a 0 para inhabilitado
                    command.Parameters.AddWithValue("@Ip", ip);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return Ok("Se pudo inhabilitar");
                    }
                    else
                    {
                        return BadRequest("No se pudo inhabilitar");
                    }
                }
            }
        }





    }

    public class DatoModelo
    {
        // Propiedades del modelo de datos
        // ...
    }

    public class ComputadoraHija
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ip { get; set; }
        public string Status { get; set; }
        public string Permiso { get; set; }
    }

    public class RegistroPrograma
    {
        public int Id { get; set; }
        public string NombrePrograma { get; set; }
        public int Pid { get; set; }
        public int timeAlive { get; set; }
        public string Usuario { get; set; }
        public string Ventana { get; set; }
        public string TituloVentana { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string ComputadoraHijaIp { get; set; }
        public int ErrorId { get; set; }
    }


}
