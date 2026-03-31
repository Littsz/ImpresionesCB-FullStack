using Microsoft.AspNetCore.Mvc;

namespace ImpresionesCB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImpresionController : ControllerBase
    {
        [HttpPost("imprimir")]
        public IActionResult ImprimirEtiquetas([FromBody] List<string> listaCodigos)
        {
            if (listaCodigos == null || listaCodigos.Count == 0)
            {
                return BadRequest("La lista de códigos está vacía.");
            }


            foreach (var codigo in listaCodigos)
            {
                Console.WriteLine($"Preparando impresión para el código: {codigo}");
            }

            return Ok(new { mensaje = "¡Conexión exitosa! La API recibió los " + listaCodigos.Count + " códigos correctamente." });
        }
    }
}