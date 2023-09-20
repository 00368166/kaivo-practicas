using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace madre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class hija : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Saludar()
        {
            return Ok("Bienvenido al servicio de la hija");
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerDato(int id)
        {
            // Obtener y retornar el dato con el ID especificado para la hija
            // ...

            // Ejemplo: Crear un objeto JSON
            var dato = new { Id = id, Nombre = "Ejemplo para la hija" };

            return Ok(dato);
        }


        [HttpPost]
        public IActionResult CrearDato([FromBody] DatoModelo dato)
        {
            // Crear el nuevo dato utilizando la información recibida en el cuerpo de la solicitud para la hija
            // ...

            return Ok("Dato creado exitosamente para la hija");
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarDato(int id, [FromBody] DatoModelo dato)
        {
            // Actualizar el dato con el ID especificado utilizando la información recibida en el cuerpo de la solicitud para la hija
            // ...

            return Ok("Dato actualizado exitosamente para la hija");
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarDato(int id)
        {
            // Eliminar el dato con el ID especificado para la hija
            // ...

            return Ok("Dato eliminado exitosamente para la hija");
        }

        // Agrega métodos adicionales específicos para la hija si es necesario

        // ...
    }
}
